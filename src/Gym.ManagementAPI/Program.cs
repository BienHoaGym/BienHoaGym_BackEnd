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
using System.Security.Claims; // Cần cái này cho ClaimTypes
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
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure(
                maxRetryCount: 5,
                maxRetryDelay: TimeSpan.FromSeconds(30),
                errorNumbersToAdd: null);
        });

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

// Configure CORS (Cho phép Frontend gọi vào)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        // Cho phép frontend ở các cổng phổ biến
        policy.WithOrigins("http://localhost:5173", "https://localhost:5173", 
                          "http://localhost:3000", "https://localhost:3000",
                          "http://localhost:3001", "https://localhost:3001")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();

// ==========================================
// 5. MIDDLEWARE PIPELINE
// ==========================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Management API V1");
        options.RoutePrefix = "swagger";
    });
}

// CORS phải đứng trước Auth
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Test Endpoint (Giúp kiểm tra nhanh)
app.MapGet("/api/debug/auth", (ClaimsPrincipal user) =>
{
    if (!user.Identity?.IsAuthenticated ?? true) return Results.Unauthorized();

    return Results.Ok(new
    {
        Name = user.Identity.Name,
        // Liệt kê các role mà hệ thống đọc được
        Roles = user.FindAll(ClaimTypes.Role).Select(c => c.Value),
        // Liệt kê tất cả claims
        AllClaims = user.Claims.Select(c => new { c.Type, c.Value })
    });
}).RequireAuthorization();

// Console Logs
Console.WriteLine("🚀 Gym Management API is running!");
Console.WriteLine("📖 Swagger UI: http://localhost:5001/swagger");

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

app.Run();