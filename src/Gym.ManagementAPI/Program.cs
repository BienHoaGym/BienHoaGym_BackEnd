using FluentValidation;
using FluentValidation.AspNetCore;
using System.IO;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Repositories;
using Gym.Application.Interfaces.Services;
using Gym.Application.Mappings;
using Gym.Application.Services;
using Gym.Application.Validators.Members;
using Gym.Domain.Entities;
using Gym.Domain.Enums;
using Gym.Infrastructure.Data;
using Gym.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authorization;
using Gym.Infrastructure.Auth;
using System.Security.Claims; // Cần cái này cho ClaimTypes
using System.Text;
using QuestPDF;

// Fix for inotify limit on Linux/Render - MUST BE AT THE VERY TOP
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "1");
Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER", "true");

QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    ContentRootPath = Directory.GetCurrentDirectory()
});

// Suppress default configuration sources that might use watchers
builder.Configuration.Sources.Clear();
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
                     .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: false)
                     .AddEnvironmentVariables();

// Fix for PostgreSQL DateTimeKind issues
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);


// ==========================================
// 1. CONFIGURATION & DATABASE
// ==========================================

// Add controllers with JSON options for camelCase
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });
builder.Services.AddEndpointsApiExplorer();

// Configure DbContext
builder.Services.AddDbContext<GymDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
        ?? Environment.GetEnvironmentVariable("DATABASE_URL");
        
    if (string.IsNullOrEmpty(connectionString))
    {
        throw new Exception("Connection string 'DefaultConnection' or 'DATABASE_URL' not found.");
    }

    // CỐ ĐỊNH: Sử dụng file DB tại thư mục chạy để đảm bảo quyền ghi và dễ quản lý
    if (connectionString.Contains(".sqlite") || connectionString.Contains("Data Source") || connectionString.Contains("Filename"))
    {
        // Sử dụng ContentRootPath trực tiếp thay vì Parent để an toàn hơn trong Docker/Render
        var dbFile = "GymManagement.sqlite";
        var sharedDbPath = Path.Combine(builder.Environment.ContentRootPath, dbFile);
        
        // Cấp cứu: Nếu file không tồn tại ở App Root, thử tìm ở Parent (tương thích ngược) hoặc Project Root
        if (!System.IO.File.Exists(sharedDbPath))
        {
            var parentPath = Path.Combine(Directory.GetParent(builder.Environment.ContentRootPath)?.FullName ?? "", dbFile);
            if (System.IO.File.Exists(parentPath)) sharedDbPath = parentPath;
        }

        Console.WriteLine($"📌 DATABASE PATH: {sharedDbPath}");
        options.UseSqlite($"Data Source={sharedDbPath}");
    }
    else
    {
        options.UseNpgsql(connectionString);
    }

    if (builder.Environment.IsDevelopment())
    {
        options.EnableSensitiveDataLogging();
        options.EnableDetailedErrors();
    }
});

// ==========================================
// 2. THIRD-PARTY LIBRARIES (AutoMapper, Validation, Swagger)
// ==========================================

// Configure AutoMapper (Sử dụng Assembly chứa MemberService để quét các Profile mapping)
builder.Services.AddAutoMapper(cfg => {}, typeof(MemberService));// Configure FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMemberValidator>();

// Configure Swagger with JWT Support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Management API",
        Version = "v1",
        Description = "Internal API for Gym Management System"
    });

    // Add JWT Authentication to Swagger UI
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Nhập 'Bearer [Token]' vào ô bên dưới.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ==========================================
// 3. DEPENDENCY INJECTION (Services & Repositories)
// ==========================================

// Register Repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// JWT Configuration Reading
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT Secret not configured");
var issuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer not configured");
var audience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience not configured");
var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

// Register JWT Service (Manual because of primitive types in constructor)
builder.Services.AddScoped<IJwtService>(sp => new JwtService(secretKey, issuer, audience, expiryMinutes));

// Register Core Services
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Register Domain Services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IPackageService, PackageService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();
builder.Services.AddScoped<ITrainerService, TrainerService>();
builder.Services.AddScoped<IClassService, ClassService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IAuditLogService, AuditLogService>();
builder.Services.AddScoped<IReportsService, ReportsService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IEquipmentService, EquipmentService>();
builder.Services.AddScoped<IEquipmentCategoryService, EquipmentCategoryService>();
builder.Services.AddScoped<IProviderService, ProviderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPdfService, Gym.Infrastructure.Services.QuestPdfService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();

// ĐĂNG KÝ CHO AUDIT LOG: Cho phép DbContext lấy được thông tin người dùng từ request hiện tại
builder.Services.AddHttpContextAccessor();

// ==========================================
// 4. AUTHENTICATION & AUTHORIZATION
// ==========================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = ClaimTypes.Role
    };
});

builder.Services.AddAuthorization();

// --- ĐĂNG KÝ PHÂN QUYỀN ĐỘNG (RBAC) ---
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

// Configure CORS (Hỗ trợ tuyệt đối cho Frontend trên GitHub Pages)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy => 
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// ==========================================
// 5. MIDDLEWARE PIPELINE
// ==========================================

// ĐƯA CORS LÊN ĐẦU TIÊN ĐỂ KHÔNG BỊ CHẶN BỞI TRÌNH DUYỆT
app.UseCors("AllowAll");

// Cấu hình bắt lỗi chi tiết để debug trên Render
app.Use(async (context, next) => {
    try {
        await next();
    } catch (Exception ex) {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        // Đảm bảo CORS luôn được phép ngay cả khi lỗi 500
        context.Response.Headers["Access-Control-Allow-Origin"] = "*";
        context.Response.Headers["Access-Control-Allow-Methods"] = "GET, POST, PUT, DELETE, OPTIONS";
        context.Response.Headers["Access-Control-Allow-Headers"] = "Content-Type, Authorization";
        
        var errorDetails = new { 
            error = "CORTEX_CRITICAL_DEBUG", 
            message = ex.Message,
            inner = ex.InnerException?.Message,
            stack = ex.StackTrace, // Thêm StackTrace để biết chính xác lỗi ở đâu
            type = ex.GetType().Name
        };
        
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorDetails));
    }
});

// --- SHARED STATIC FILES FOR UPLOADS ---
// Sử dụng thư mục uploads ngay tại App Root để đảm bảo Render map đúng volume (nếu có)
var uploadsPath = Path.Combine(builder.Environment.ContentRootPath, "uploads");
if (!Directory.Exists(uploadsPath)) 
{
    Directory.CreateDirectory(uploadsPath);
}
Console.WriteLine($"📂 UPLOADS PATH: {uploadsPath}");

app.UseStaticFiles(); // Theo mặc định là wwwroot
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new Microsoft.Extensions.FileProviders.PhysicalFileProvider(uploadsPath),
    RequestPath = "/uploads"
});

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Master API V1");
    options.RoutePrefix = "swagger";
});

// Thêm route gốc để kiểm tra nhanh trạng thái API
app.MapGet("/", () => new { 
    service = "Gym Master API", 
    status = "Running", 
    swagger = "/swagger",
    health = "/api/health",
    timestamp = DateTime.UtcNow 
});

// Thêm Endpoint Health Check công khai (Không cần Auth) để kiểm tra CORS/DB
app.MapGet("/api/health", async (GymDbContext db) => {
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { 
        status = "Online", 
        database = canConnect ? "Connected" : "Disconnected",
        time = DateTime.UtcNow 
    });
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Test Endpoint (Giúp kiểm tra nhanh)
app.MapGet("/api/debug/auth", (ClaimsPrincipal user) =>
{
    if (!user.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();

    return Results.Ok(new
    {
        Name = user.Identity!.Name,
        // Liệt kê các role mà hệ thống đọc được
        Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value),
        // Liệt kê tất cả claims
        AllClaims = user.Claims.Select(c => new { c.Type, c.Value })
    });
}).RequireAuthorization();

// Console Logs
Console.WriteLine("🚀 Gym Management API is running!");
Console.WriteLine("📖 Swagger UI: /swagger");
Console.WriteLine("DB: " + builder.Configuration.GetConnectionString("DefaultConnection"));

// Auto Migrate & Seed (Bọc trong try-catch an toàn để tránh sập App)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try 
    {
        var db = services.GetRequiredService<GymDbContext>();
        Console.WriteLine("🔄 Starting Database Sync & Migration...");
        
        // --- CỨU HỎA: TỰ ĐỘNG SỬA BẢNG NẾU THIẾU CỘT (DO LỖI SYNC TRÊN POSTGRES) ---
        if (db.Database.IsNpgsql())
        {
            try {
                Console.WriteLine("🛠️ Running Database self-healing (Postgres)...");
                db.Database.ExecuteSqlRaw(@"
                    DO $$ 
                    BEGIN 
                        -- Sửa bảng Users (Thêm các cột nhân sự thiếu)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='Address') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""Address"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='IdentityNumber') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""IdentityNumber"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='BirthDate') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""BirthDate"" timestamp with time zone;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='Gender') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""Gender"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='HireDate') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""HireDate"" timestamp with time zone;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='BankCardNumber') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""BankCardNumber"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='BankName') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""BankName"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='ProfilePhoto') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""ProfilePhoto"" text;
                        END IF;

                        -- Sửa bảng MemberSubscriptions
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='AutoPauseExtensionDays') THEN
                            ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""AutoPauseExtensionDays"" integer;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='LastPausedAt') THEN
                            ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""LastPausedAt"" timestamp with time zone;
                        END IF;
                        
                        -- Sửa bảng Invoices (Thêm các cột audit)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Invoices' AND column_name='CreatedByUserId') THEN
                            ALTER TABLE ""Invoices"" ADD COLUMN ""CreatedByUserId"" uuid;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Invoices' AND column_name='CreatedByUserName') THEN
                            ALTER TABLE ""Invoices"" ADD COLUMN ""CreatedByUserName"" text;
                        END IF;

                        -- Sửa bảng InvoiceDetails
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='InvoiceDetails' AND column_name='SubscriptionId') THEN
                            ALTER TABLE ""InvoiceDetails"" ADD COLUMN ""SubscriptionId"" uuid;
                        END IF;

                        -- Sửa bảng MembershipPackages (Duration rename)
                        IF EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MembershipPackages' AND column_name='DurationDays') THEN
                            ALTER TABLE ""MembershipPackages"" RENAME COLUMN ""DurationDays"" TO ""DurationInDays"";
                        END IF;

                        -- Sửa bảng Providers (Công nợ)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Providers' AND column_name='TotalDebt') THEN
                            ALTER TABLE ""Providers"" ADD COLUMN ""TotalDebt"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Providers' AND column_name='SupplyType') THEN
                            ALTER TABLE ""Providers"" ADD COLUMN ""SupplyType"" text;
                        END IF;

                        -- Sửa bảng StockTransactions (Chi tiết giao dịch kho)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='PaidAmount') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""PaidAmount"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='TotalAmount') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""TotalAmount"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='VatPercentage') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""VatPercentage"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='PaymentMethod') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""PaymentMethod"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='PaymentDueDate') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""PaymentDueDate"" timestamp with time zone;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='AttachmentUrl') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""AttachmentUrl"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='ReferenceNumber') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""ReferenceNumber"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='ExpiryDate') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""ExpiryDate"" timestamp with time zone;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='StockTransactions' AND column_name='ProviderId') THEN
                            ALTER TABLE ""StockTransactions"" ADD COLUMN ""ProviderId"" uuid;
                        END IF;

                        -- Sửa bảng EquipmentTransactions (Giao dịch thiết bị)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='EquipmentTransactions' AND column_name='PaidAmount') THEN
                            ALTER TABLE ""EquipmentTransactions"" ADD COLUMN ""PaidAmount"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='EquipmentTransactions' AND column_name='TotalAmount') THEN
                            ALTER TABLE ""EquipmentTransactions"" ADD COLUMN ""TotalAmount"" decimal DEFAULT 0;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='EquipmentTransactions' AND column_name='ProviderId') THEN
                            ALTER TABLE ""EquipmentTransactions"" ADD COLUMN ""ProviderId"" uuid;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='EquipmentTransactions' AND column_name='AttachmentUrl') THEN
                            ALTER TABLE ""EquipmentTransactions"" ADD COLUMN ""AttachmentUrl"" text;
                        END IF;

                        -- Sửa bảng TrainerMemberAssignments (Workflow PT 1-1)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='TrainerMemberAssignments' AND column_name='MemberSubscriptionId') THEN
                            ALTER TABLE ""TrainerMemberAssignments"" ADD COLUMN ""MemberSubscriptionId"" uuid;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='TrainerMemberAssignments' AND column_name='Status') THEN
                            ALTER TABLE ""TrainerMemberAssignments"" ADD COLUMN ""Status"" integer DEFAULT 1;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Trainers' AND column_name='IsPublic') THEN
                            ALTER TABLE ""Trainers"" ADD COLUMN ""IsPublic"" boolean DEFAULT false;
                        END IF;

                        -- TỰ CỨU: Tạo bảng Kiểm kê (StockAudits & StockAuditDetails) nếu chưa có
                        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'StockAudits') THEN
                            CREATE TABLE ""StockAudits"" (
                                ""Id"" uuid PRIMARY KEY,
                                ""WarehouseId"" uuid NOT NULL,
                                ""AuditDate"" timestamp with time zone NOT NULL,
                                ""PerformedBy"" text,
                                ""ApprovedBy"" text,
                                ""Status"" integer NOT NULL,
                                ""Note"" text,
                                ""CreatedAt"" timestamp with time zone NOT NULL,
                                ""UpdatedAt"" timestamp with time zone,
                                ""IsDeleted"" boolean NOT NULL DEFAULT false
                            );
                        END IF;

                        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'StockAuditDetails') THEN
                            CREATE TABLE ""StockAuditDetails"" (
                                ""Id"" uuid PRIMARY KEY,
                                ""StockAuditId"" uuid NOT NULL,
                                ""ProductId"" uuid NOT NULL,
                                ""SystemQuantity"" integer NOT NULL,
                                ""ActualQuantity"" integer NOT NULL,
                                ""Reason"" text,
                                ""CreatedAt"" timestamp with time zone NOT NULL,
                                ""UpdatedAt"" timestamp with time zone,
                                ""IsDeleted"" boolean NOT NULL DEFAULT false
                            );
                        END IF;

                        IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'ProviderPayments') THEN
                            CREATE TABLE ""ProviderPayments"" (
                                ""Id"" uuid PRIMARY KEY,
                                ""ProviderId"" uuid NOT NULL,
                                ""Amount"" decimal(18,2) NOT NULL,
                                ""PaymentMethod"" text NOT NULL,
                                ""Date"" timestamp with time zone NOT NULL,
                                ""Note"" text,
                                ""ReferenceNumber"" text,
                                ""StockTransactionId"" uuid,
                                ""EquipmentTransactionId"" uuid,
                                ""PerformedBy"" text,
                                ""CreatedAt"" timestamp with time zone NOT NULL,
                                ""UpdatedAt"" timestamp with time zone,
                                ""IsDeleted"" boolean NOT NULL DEFAULT false
                            );
                        END IF;

                        -- Sửa bảng Members (Thêm các cột mới)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='Gender') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""Gender"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='Address') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""Address"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='Note') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""Note"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='EmergencyContact') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""EmergencyContact"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='EmergencyPhone') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""EmergencyPhone"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='FaceEncoding') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""FaceEncoding"" text;
                        END IF;
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Members' AND column_name='QRCode') THEN
                            ALTER TABLE ""Members"" ADD COLUMN ""QRCode"" text;
                        END IF;

                        -- Khởi tạo giá trị mặc định cho dữ liệu cũ (Tránh lỗi NULL)
                        UPDATE ""Providers"" SET ""TotalDebt"" = 0 WHERE ""TotalDebt"" IS NULL;
                        UPDATE ""Providers"" SET ""SupplyType"" = 'General' WHERE ""SupplyType"" IS NULL;
                        
                        UPDATE ""StockTransactions"" SET ""PaidAmount"" = 0 WHERE ""PaidAmount"" IS NULL;
                        UPDATE ""StockTransactions"" SET ""TotalAmount"" = 0 WHERE ""TotalAmount"" IS NULL;
                        UPDATE ""StockTransactions"" SET ""VatPercentage"" = 0 WHERE ""VatPercentage"" IS NULL;
                        
                        UPDATE ""EquipmentTransactions"" SET ""PaidAmount"" = 0 WHERE ""PaidAmount"" IS NULL;
                        UPDATE ""EquipmentTransactions"" SET ""TotalAmount"" = 0 WHERE ""TotalAmount"" IS NULL;
                    END $$;");
                Console.WriteLine("✅ Database self-healing completed!");
            } catch (Exception ex) {
                Console.WriteLine($"🔍 Self-healing info: {ex.Message}");
            }
        }
        else if (db.Database.ProviderName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            try {
                Console.WriteLine("🛠️ Running Database self-healing (SQLite)...");
                // SQLite ALTER TABLE ADD doesn't support IF NOT EXISTS easily, so we use try-catch for each
                try { db.Database.ExecuteSqlRaw("ALTER TABLE TrainerMemberAssignments ADD COLUMN MemberSubscriptionId TEXT;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE TrainerMemberAssignments ADD COLUMN Status INTEGER DEFAULT 1;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Users ADD COLUMN ProfilePhoto TEXT;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Trainers ADD COLUMN IsPublic INTEGER DEFAULT 0;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Members ADD COLUMN FaceEncoding TEXT;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Members ADD COLUMN QRCode TEXT;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Members ADD COLUMN Gender TEXT;"); } catch {}
                try { db.Database.ExecuteSqlRaw("ALTER TABLE Members ADD COLUMN Address TEXT;"); } catch {}
                Console.WriteLine("✅ SQLite self-healing completed!");
            } catch (Exception ex) {
                Console.WriteLine($"🔍 SQLite healing info: {ex.Message}");
            }
        }
        else 
        {
            Console.WriteLine("ℹ️ Skipping self-healing (Provider: " + db.Database.ProviderName + ")");
        }

        // Log danh sách migration đã áp dụng
        var appliedMigrations = db.Database.GetAppliedMigrations();
        Console.WriteLine($"📊 Applied Migrations: {string.Join(", ", appliedMigrations)}");
        
        db.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully!");

        // --- PT ASSIGNMENT RECOVERY (CẤP CỨU: Tự động khôi phục các hợp đồng PT bị thiếu từ POS) ---
        try {
            Console.WriteLine("🔍 Checking for missing PT assignments...");
            var missingAssignments = await db.MemberSubscriptions
                .Include(s => s.Package)
                .Where(s => s.Status == SubscriptionStatus.Active && s.Package.HasPT && !s.IsDeleted)
                .Where(s => !db.TrainerMemberAssignments.Any(a => a.MemberSubscriptionId == s.Id))
                .ToListAsync();

            if (missingAssignments.Any())
            {
                // Lấy PT đầu tiên làm mặc định nếu DB yêu cầu NOT NULL (như trên SQLite cũ)
                var defaultTrainer = await db.Trainers.FirstOrDefaultAsync(t => !t.IsDeleted && t.IsActive);

                foreach (var sub in missingAssignments)
                {
                    var ptContract = new TrainerMemberAssignment
                    {
                        MemberId = sub.MemberId,
                        MemberSubscriptionId = sub.Id,
                        Status = TrainerAssignmentStatus.PendingAssignment,
                        AssignedDate = DateTime.UtcNow,
                        IsActive = true,
                        Notes = $"Hợp đồng tự động (Phục hồi từ hệ thống): {sub.Package.Name}"
                    };

                    // Nếu là SQLite, gán tạm PT mặc định để vượt qua ràng buộc NOT NULL nếu có
                    if (db.Database.IsSqlite() && defaultTrainer != null)
                    {
                        ptContract.TrainerId = defaultTrainer.Id;
                        ptContract.Notes += " (Gán tạm PT do ràng buộc DB)";
                    }

                    await db.TrainerMemberAssignments.AddAsync(ptContract);
                }
                await db.SaveChangesAsync();
                Console.WriteLine($"✅ Fixed {missingAssignments.Count} missing PT assignments!");
            }
        } catch (Exception ex) {
            Console.WriteLine($"⚠️ PT Recovery Warning: {ex.Message}");
        }

        // 🟢 LOGIC TỰ ĐỘNG DỌN DẸP DỮ LIỆU LỖI PHÔNG CHỮ (BÃ¡n lÃ° -> Bán lẻ, GA3i -> Gói)
        try {
            var connection = db.Database.GetDbConnection();
            await connection.OpenAsync();
            using var command = connection.CreateCommand();
            command.CommandText = @"
                -- Sửa tên gói trong MembershipPackages
                UPDATE MembershipPackages SET Name = REPLACE(Name, 'GA3i', 'Gói');
                UPDATE MembershipPackages SET Name = REPLACE(Name, 'ThAng', 'Tháng');
                UPDATE MembershipPackages SET Name = REPLACE(Name, 'Nm', 'Năm');

                -- Sửa tên gói trong Subscriptions
                UPDATE MemberSubscriptions SET OriginalPackageName = REPLACE(OriginalPackageName, 'GA3i', 'Gói');
                UPDATE MemberSubscriptions SET OriginalPackageName = REPLACE(OriginalPackageName, 'ThAng', 'Tháng');
                
                -- Sửa tên sản phẩm và dịch vụ
                UPDATE Products SET Name = 'Nước suối Aquafina' WHERE Name LIKE '%Aquafina%';
                UPDATE Products SET Name = 'Whey Protein Gold' WHERE Name LIKE '%Whey%';
                UPDATE Products SET Category = 'Thực phẩm bổ sung' WHERE Category LIKE '%Th%c ph%cm%';
                UPDATE Products SET Category = 'Đồ uống' WHERE Category LIKE '%u%ng%';
                UPDATE Products SET Unit = 'Cái' WHERE Unit = 'CAi';

                -- Sửa lỗi 'Bán lẻ' bị lỗi thành 'BÃ¡n lÃ°'
                -- UPDATE Invoices SET MemberName = 'Khách lẻ' WHERE MemberName LIKE '%Kh%ch l%';
                UPDATE Invoices SET Note = 'Bán lẻ' WHERE Note LIKE '%B%n l%';

                -- Cập nhật lại các hóa đơn Demo trong báo cáo (Dựa vào hình ảnh)
                UPDATE InvoiceDetails SET ItemName = REPLACE(ItemName, 'GA3i', 'Gói');
                UPDATE InvoiceDetails SET ItemName = REPLACE(ItemName, 'ThAng', 'Tháng');
            ";
            await command.ExecuteNonQueryAsync();
            Console.WriteLine("✨ Database Cleanup: Corrupted characters fixed!");
        } catch (Exception ex) {
            Console.WriteLine($"⚠️ Database Cleanup Warning: {ex.Message}");
        }

        // 🚨 FORCED ADMIN SYNC (Cấp cứu đăng nhập cho bản Deployed)
        var systemAdmin = await db.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Username.ToLower() == "admin");
        if (systemAdmin != null)
        {
            // Forcing known password for rescue: Admin@123
            var hasher = new Microsoft.AspNetCore.Identity.PasswordHasher<Gym.Domain.Entities.User>();
            systemAdmin.PasswordHash = hasher.HashPassword(systemAdmin, "Admin@123");
            
            // Ensure has Admin role (Id = 1)
            if (!systemAdmin.UserRoles.Any(ur => ur.RoleId == 1))
            {
                systemAdmin.UserRoles.Add(new Gym.Domain.Entities.UserRole { RoleId = 1, UserId = systemAdmin.Id });
            }
            await db.SaveChangesAsync();
            Console.WriteLine("🚑 Rescue Mode: System 'admin' password synced to 'Admin@123'");
        }
        
        // 🔄 ĐỒNG BỘ QUYỀN TRUY CẬP CHO VAI TRÒ ADMIN (Đảm bảo không bị mất Sidebar)
        var adminRoles = await db.Roles.Where(r => r.RoleName == "Admin").ToListAsync();
        foreach (var role in adminRoles)
        {
            if (string.IsNullOrEmpty(role.Permissions) || role.Permissions == "[]")
            {
                role.Permissions = "[\"*\"]";
                _ = db.Roles.Update(role);
                Console.WriteLine($"🛡️ Security Sync: Full permissions restored for role '{role.RoleName}'");
            }
        }
        await db.SaveChangesAsync();

        if (systemAdmin == null)
        {
            Console.WriteLine("⚠️ No users found in database. Creating default Admin for first-time setup...");
            await Gym.Infrastructure.Data.DataSeeder.SeedDefaultAdminAsync(services);
            Console.WriteLine("✅ Default Admin created (admin / 123456)");
        }

        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("🌱 Seeding demo data in Development...");
            await Gym.Infrastructure.Data.DataSeeder.SeedDemoDataAsync(services);
            Console.WriteLine("✅ Seeding completed!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Database Initialization bypass: {ex.Message}");
        // Không quăng lỗi ra ngoài để App tiếp tục chạy
    }
}

// Database Check
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    try
    {
        if (db.Database.CanConnect())
            Console.WriteLine("✅ Database connected successfully!");
        else
            Console.WriteLine("❌ Cannot connect to database!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Database error: {ex.Message}");
    }
}

// Lấy PORT từ môi trường (Render cấp)
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";

Console.WriteLine($"📍 Port: {port}");

app.Run($"http://0.0.0.0:{port}");
