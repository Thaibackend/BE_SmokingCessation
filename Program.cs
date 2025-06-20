using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SmokingQuitSupportAPI.Data;
using SmokingQuitSupportAPI.Data.Repositories;
using SmokingQuitSupportAPI.Data.Repositories.Interfaces;
using SmokingQuitSupportAPI.Services;
using SmokingQuitSupportAPI.Mappings;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", 
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

// Configure Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo 
    { 
        Title = "Smoking Quit Support API", 
        Version = "v1",
        Description = "API for Smoking Quit Support Application"
    });
    
    // Configure JWT Authentication in Swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT Authentication
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"] ?? "");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception.Message}");
                Console.WriteLine($"Token: {context.Request.Headers.Authorization}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                var claims = context.Principal?.Claims.Select(c => $"{c.Type}: {c.Value}");
                Console.WriteLine($"Claims: {string.Join(", ", claims ?? new string[0])}");
                return Task.CompletedTask;
            },
            OnMessageReceived = context =>
            {
                Console.WriteLine($"Token received: {context.Token}");
                return Task.CompletedTask;
            }
        };
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero
        };
    });

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IActivityRepository, ActivityRepository>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IPackageRepository, PackageRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// New repository registrations
builder.Services.AddScoped<IAchievementRepository, AchievementRepository>();
builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
builder.Services.AddScoped<ISmokingStatusRepository, SmokingStatusRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IDailyTaskRepository, DailyTaskRepository>();
builder.Services.AddScoped<ICoachApplicationRepository, CoachApplicationRepository>();
builder.Services.AddScoped<IPlanTemplateRepository, PlanTemplateRepository>();

// Add services
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ActivityService>();
builder.Services.AddScoped<PostService>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<PackageService>();
builder.Services.AddScoped<PlanService>();
builder.Services.AddScoped<OrderService>();

// New service registrations
builder.Services.AddScoped<AchievementService>();
builder.Services.AddScoped<SmokingStatusService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<DailyTaskService>();
builder.Services.AddScoped<CoachApplicationService>();
builder.Services.AddScoped<PlanTemplateService>();

var app = builder.Build();

// Configure the HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Smoking Quit Support API V1");
    c.RoutePrefix = string.Empty; // Để Swagger UI ở root path
});

app.UseCors("AllowAll");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// Test database connection
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        await context.Database.CanConnectAsync();
        Console.WriteLine("Database connection successful!");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database connection failed: {ex.Message}");
    }
}

app.Run(); 