using FluentValidation;
using FluentValidation.AspNetCore;
using Gym.Application.Interfaces;
using Gym.Application.Interfaces.Repositories;
using Gym.Application.Interfaces.Services;
using Gym.Application.Mappings;
using Gym.Application.Services;
using Gym.Application.Validators.Members;
using Gym.Domain.Entities;
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

// Fix for inotify limit on Linux/Render - MUST BE AT THE VERY TOP
Environment.SetEnvironmentVariable("DOTNET_USE_POLLING_FILE_WATCHER", "1");
Environment.SetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER", "true");

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

// Add controllers
builder.Services.AddControllers();
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

    // Tự động nhận diện Provider: Nếu là file .sqlite hoặc chuỗi cho SQLite thì dùng UseSqlite
    if (connectionString.Contains(".sqlite") || connectionString.Contains("Data Source") || connectionString.Contains("Filename"))
    {
        // LOGIC SỬA ĐƯỜNG DẪN SQLITE CHO RENDER
        var sqliteFile = "GymManagement.sqlite";
        if (connectionString.Contains("Data Source=")) {
            sqliteFile = connectionString.Split("Data Source=")[1].Split(';')[0];
        }

        // Kiểm tra các đường dẫn có thể tồn tại trên Render
        var possiblePaths = new[] {
            sqliteFile,
            Path.Combine("src", "Gym.ManagementAPI", sqliteFile),
            Path.Combine(Directory.GetCurrentDirectory(), sqliteFile),
            Path.Combine(Directory.GetCurrentDirectory(), "src", "Gym.ManagementAPI", sqliteFile)
        };

        var actualPath = possiblePaths.FirstOrDefault(p => File.Exists(p)) ?? sqliteFile;
        Console.WriteLine($"🔍 Using SQLite at: {Path.GetFullPath(actualPath)}");
        
        options.UseSqlite($"Data Source={actualPath}");
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
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Management API V1");
    options.RoutePrefix = "swagger";
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
        Console.WriteLine("🔄 Starting Database Migration...");
        
        // Log danh sách migration đã áp dụng
        var appliedMigrations = db.Database.GetAppliedMigrations();
        Console.WriteLine($"📊 Applied Migrations: {string.Join(", ", appliedMigrations)}");
        
        db.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully!");

        // --- CỨU HỎA: TỰ ĐỘNG SỬA BẢNG NẾU THIẾU CỘT (DO LỖI SYNC) ---
        try {
            Console.WriteLine("🛠️ Checking for missing columns in MemberSubscriptions...");
            db.Database.ExecuteSqlRaw(@"
                DO $$ 
                BEGIN 
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='AutoPauseExtensionDays') THEN
                        ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""AutoPauseExtensionDays"" integer;
                        RAISE NOTICE 'Added AutoPauseExtensionDays column';
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='LastPausedAt') THEN
                        ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""LastPausedAt"" timestamp with time zone;
                        RAISE NOTICE 'Added LastPausedAt column';
                    END IF;
                END $$;");
            Console.WriteLine("✅ Database self-healing completed!");
        } catch (Exception ex) {
            Console.WriteLine($"🔍 Self-healing info: {ex.Message}");
        }

        // CHỈ SEED DỮ LIỆU NẾU LÀ MÔI TRƯỜNG DEVELOPMENT 
        // Trên Production/Render chúng ta đã có file SQLite đi kèm hoặc dùng DB ngoài
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
