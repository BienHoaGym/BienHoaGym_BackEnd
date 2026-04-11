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

    // CỐ ĐỊNH: Chỉ sử dụng 1 file duy nhất tại thư mục chạy để tránh dữ liệu bị "lan mang"
    if (connectionString.Contains(".sqlite") || connectionString.Contains("Data Source") || connectionString.Contains("Filename"))
    {
        var dbPath = Path.Combine(builder.Environment.ContentRootPath, "GymManagement.sqlite");
        Console.WriteLine($"📌 PUBLIC API DATABASE FIXED PATH: {dbPath}");
        options.UseSqlite($"Data Source={dbPath}");
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

builder.Services.AddAutoMapper(cfg => {}, typeof(MemberService));
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateMemberValidator>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Public API",
        Version = "v1",
        Description = "Public and Service API for Gym Management System"
    });

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
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ==========================================
// 3. DEPENDENCY INJECTION
// ==========================================

builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<ITrainerRepository, TrainerRepository>();
builder.Services.AddScoped<IClassRepository, ClassRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new Exception("JWT Secret not configured");
var issuer = jwtSettings["Issuer"] ?? throw new Exception("JWT Issuer not configured");
var audience = jwtSettings["Audience"] ?? throw new Exception("JWT Audience not configured");
var expiryMinutes = int.Parse(jwtSettings["ExpiryMinutes"] ?? "60");

builder.Services.AddScoped<IJwtService>(sp => new JwtService(secretKey, issuer, audience, expiryMinutes));
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Domain Services
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

builder.Services.AddHttpContextAccessor();

// ==========================================
// 4. AUTHENTICATION & AUTHORIZATION
// ==========================================

builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options => {
    options.TokenValidationParameters = new TokenValidationParameters {
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
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionHandler>();

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

var app = builder.Build();

// ==========================================
// 5. MIDDLEWARE PIPELINE
// ==========================================

app.UseCors("AllowAll");

app.Use(async (context, next) => {
    try {
        await next();
    } catch (Exception ex) {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
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

app.UseSwagger();
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Public API V1");
    options.RoutePrefix = "swagger";
});

// Health Check
app.MapGet("/api/health", async (GymDbContext db) => {
    var canConnect = await db.Database.CanConnectAsync();
    return Results.Ok(new { status = "Online", database = canConnect ? "Connected" : "Disconnected", time = DateTime.UtcNow });
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => new { service = "Gym Public API", status = "Running", swagger = "/swagger", timestamp = DateTime.UtcNow });

// Auto Migrate & Seed
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var db = services.GetRequiredService<GymDbContext>();
    try {
        Console.WriteLine("🔄 Starting Public API Database Sync...");
        
        if (db.Database.IsNpgsql()) {
            try {
                db.Database.ExecuteSqlRaw(@"
                    DO $$ 
                    BEGIN 
                        -- Sửa bảng Users
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Users' AND column_name='Address') THEN
                            ALTER TABLE ""Users"" ADD COLUMN ""Address"" text;
                        END IF;
                        -- (Cac cot khac tuong tu nhu trong Management)
                        IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name='Invoices' AND column_name='CreatedByUserId') THEN
                            ALTER TABLE ""Invoices"" ADD COLUMN ""CreatedByUserId"" uuid;
                        END IF;
                    END $$;");
            } catch { }
        }
        
        db.Database.Migrate();

        // 🚨 ADMIN & PERMISSION SYNC (ĐỒNG BỘ NỀN TẢNG)
        var systemAdmin = await db.Users.Include(u => u.UserRoles).FirstOrDefaultAsync(u => u.Username.ToLower() == "admin");
        if (systemAdmin != null) {
            var hasher = new PasswordHasher<User>();
            systemAdmin.PasswordHash = hasher.HashPassword(systemAdmin, "Admin@123");
            await db.SaveChangesAsync();
            Console.WriteLine("🚑 Public API: Admin rescue password synced.");
        }

        var adminRoles = await db.Roles.Where(r => r.RoleName == "Admin").ToListAsync();
        foreach (var role in adminRoles) {
            if (string.IsNullOrEmpty(role.Permissions) || role.Permissions == "[]") {
                role.Permissions = "[\"*\"]";
                db.Roles.Update(role);
            }
        }
        await db.SaveChangesAsync();
        Console.WriteLine("🛡️ Public API: RBAC sync completed.");

        var userCount = await db.Users.CountAsync();
        if (userCount == 0) {
            await DataSeeder.SeedDefaultAdminAsync(services);
            Console.WriteLine("✅ Default Admin created via Public API");
        }
    } catch (Exception ex) {
        Console.WriteLine($"❌ Initialization error: {ex.Message}");
    }
}

var port = Environment.GetEnvironmentVariable("PORT") ?? "10001"; // Default different port if local
app.Run($"http://0.0.0.0:{port}");
