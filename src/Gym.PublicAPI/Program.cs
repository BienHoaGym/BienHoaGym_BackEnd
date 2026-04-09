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
using System.Security.Claims;
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
        options.UseSqlite(connectionString);
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

// Configure AutoMapper
builder.Services.AddAutoMapper(cfg => {}, typeof(MemberService));
// Configure FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMemberValidator>();

// Configure Swagger with JWT Support
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Public API (Production)",
        Version = "v1",
        Description = "Public faces for Gym Management System with full business logic"
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

// Register JWT Service
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

// ĐĂNG KÝ CHO AUDIT LOG
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

// RBAC
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

// Configure CORS
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

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Public API V1");
    options.RoutePrefix = "swagger";
});

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
            stack = ex.StackTrace,
            type = ex.GetType().Name
        };
        
        await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(errorDetails));
    }
});
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Test endpoint
app.MapGet("/", () => new
{
    service = "Gym Public API",
    version = "1.0",
    status = "Running",
    description = "Public API for Marketing Website",
    swagger = "/swagger",
    timestamp = DateTime.UtcNow
});

Console.WriteLine("🌐 Gym Public API is running!");
Console.WriteLine("📖 Swagger UI: /swagger");
Console.WriteLine("DB: " + builder.Configuration.GetConnectionString("DefaultConnection"));

// Auto Migrate & Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
        var db = services.GetRequiredService<GymDbContext>();
        try 
        {
            Console.WriteLine("🔄 Starting Database Sync & Migration...");
            
            // --- CỨU HỎA: TỰ ĐỘNG SỬA BẢNG NẾU THIẾU CỘT (DO LỖI SYNC) ---
            try {
                Console.WriteLine("🛠️ Running Database self-healing...");
                db.Database.ExecuteSqlRaw(@"
                    DO $$ 
                    BEGIN 
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='AutoPauseExtensionDays') THEN
                            ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""AutoPauseExtensionDays"" integer;
                        END IF;
                        
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='MemberSubscriptions' AND column_name='LastPausedAt') THEN
                            ALTER TABLE ""MemberSubscriptions"" ADD COLUMN ""LastPausedAt"" timestamp with time zone;
                        END IF;
                        
                        -- Sửa bảng InvoiceDetails
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='InvoiceDetails' AND column_name='SubscriptionId') THEN
                            ALTER TABLE ""InvoiceDetails"" ADD COLUMN ""SubscriptionId"" uuid;
                        END IF;
                    END $$;");
                Console.WriteLine("✅ Database self-healing completed!");
            } catch (Exception ex) {
                Console.WriteLine($"🔍 Self-healing info: {ex.Message}");
            }

            // Log danh sách migration đã áp dụng
            var appliedMigrations = db.Database.GetAppliedMigrations();
            Console.WriteLine($"📊 Applied Migrations: {string.Join(", ", appliedMigrations)}");
            
            db.Database.Migrate();
            Console.WriteLine("✅ Database migrated successfully!");

        if (app.Environment.IsDevelopment())
        {
            await Gym.Infrastructure.Data.DataSeeder.SeedDemoDataAsync(services);
            Console.WriteLine("✅ Seeding completed!");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Initialization error: {ex.Message}");
    }
}

// Lấy PORT từ môi trường
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
Console.WriteLine($"📍 Port: {port}");

app.Run($"http://0.0.0.0:{port}");
