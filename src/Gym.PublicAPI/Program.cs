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

var builder = WebApplication.CreateBuilder(args);

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
        policy.WithOrigins("https://bienhoagym.github.io", "http://localhost:5173", "http://localhost:5174", "http://localhost:5175", "http://localhost:3000")
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

// Auto Migrate
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<GymDbContext>();
    try 
    {
        db.Database.Migrate();
        Console.WriteLine("✅ Database migrated successfully!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Migration error: {ex.Message}");
    }
}

// Lấy PORT từ môi trường
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
Console.WriteLine($"📍 Port: {port}");

app.Run($"http://0.0.0.0:{port}");
