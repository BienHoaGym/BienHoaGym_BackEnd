using Gym.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Gym Public API",
        Version = "v1",
        Description = "Public API for Gym Marketing Website",
        Contact = new OpenApiContact
        {
            Name = "Gym Management Team",
            Email = "support@gymmgmt.com"
        }
    });
});

// Configure DbContext with SQL Server
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

// Configure CORS - Public API cần CORS rộng hơn
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

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Gym Public API V1");
        options.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowAll");
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
Console.WriteLine("📖 Swagger UI: http://localhost:5002/swagger");
Console.WriteLine("📍 Port: 5002");

app.Run();