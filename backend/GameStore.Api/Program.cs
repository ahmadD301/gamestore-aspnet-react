using GameStore.Api.Configuration;
using GameStore.Api.Extensions;
using GameStore.Data.Contexts;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GameStore.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using GameStore.Api.Services.Auth;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Database connection string is missing.");

var jwtSettings = builder.Configuration
                    .GetSection(JwtSettings.SectionName)
                    .Get<JwtSettings>()
                    ?? throw new InvalidOperationException(
                        "JWT settings are missing.");

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});
// Identity
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = true;

        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Authentication (JWT Bearer)
// NOTE: AddIdentity registers cookie auth schemes and can override default
// AuthenticationOptions. Configure Bearer AFTER Identity so `[Authorize]`
// uses JWT by default for API endpoints.
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,

                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtSettings.Key)),

                RoleClaimType = ClaimTypes.Role
            };

        // Dev-friendly diagnostics (doesn't log the token itself)
        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var logger = context.HttpContext
                    .RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");

                var authHeader =
                    context.Request.Headers.Authorization.ToString();

                if (string.IsNullOrWhiteSpace(authHeader))
                {
                    logger.LogDebug(
                        "No Authorization header present for {Method} {Path}",
                        context.Request.Method,
                        context.Request.Path);
                }
                else
                {
                    var isDoubleBearer = authHeader
                        .StartsWith("Bearer Bearer ",
                            StringComparison.OrdinalIgnoreCase);

                    logger.LogDebug(
                        "Authorization header received for {Method} {Path}. StartsWithBearer={StartsWithBearer}, DoubleBearer={DoubleBearer}, Length={Length}",
                        context.Request.Method,
                        context.Request.Path,
                        authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase),
                        isDoubleBearer,
                        authHeader.Length);
                }

                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                var logger = context.HttpContext
                    .RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");

                logger.LogWarning(
                    context.Exception,
                    "JWT authentication failed for {Method} {Path}",
                    context.Request.Method,
                    context.Request.Path);

                return Task.CompletedTask;
            },
            OnChallenge = context =>
            {
                var logger = context.HttpContext
                    .RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("JwtBearer");

                if (!string.IsNullOrWhiteSpace(context.Error) ||
                    !string.IsNullOrWhiteSpace(context.ErrorDescription))
                {
                    logger.LogDebug(
                        "JWT challenge for {Method} {Path}. Error={Error} Description={Description}",
                        context.Request.Method,
                        context.Request.Path,
                        context.Error,
                        context.ErrorDescription);
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization();

// Services
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "GameStore API Docs";

        options.SwaggerEndpoint(
            "/swagger/v1/swagger.json",
            "GameStore API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateAsyncScope())
{
    var services = scope.ServiceProvider;

    var context =
        services.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    var roleManager =
    services.GetRequiredService<
        RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<
            UserManager<ApplicationUser>>();

    await IdentitySeed.SeedRolesAsync(roleManager);

    await IdentitySeed.SeedAdminAsync(userManager);

    await ApplicationDbContextSeed.SeedAsync(context);
}

app.Run();