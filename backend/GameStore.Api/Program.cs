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
using FluentValidation;
using FluentValidation.AspNetCore;
using GameStore.Api.Exceptions;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using GameStore.Api.Middleware;
using Microsoft.AspNetCore.RateLimiting;


Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File(
        "logs/gamestore-.txt",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

var builder =
    WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// IMPORTANT: Do NOT call builder.Environment.IsEnvironment() here.
// When WebApplicationFactory uses HostFactoryResolver it runs Program.cs
// via reflection BEFORE ConfigureWebHost (and therefore before
// UseEnvironment("Testing")) is called. builder.Environment reflects the
// host's environment at the time Program.cs executes — which is still
// "Production" or whatever ASPNETCORE_ENVIRONMENT is set to in the shell.
// Any isTesting guard based on builder.Environment is therefore always
// false during test startup and provides no protection.
//
// The only safe approach: never throw during configuration reads.
// Validate presence/validity only after the DI container is fully built
// (i.e. inside a hosted service or the app.Run() block), or rely on
// Options validation (AddOptions<T>().Validate(...)).

builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

// Connection string — null is acceptable here; AddDbContext below
// only registers UseSqlServer when the string is present.
var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection");

// JWT settings — read without throwing. An empty/missing section produces
// a default JwtSettings instance (empty strings). The JWT bearer setup
// below will use whatever values are present; in Testing the factory's
// ConfigureAppConfiguration fills them in before the host actually starts
// processing requests (even though it runs after this line executes,
// TokenValidationParameters are evaluated per-request, not at startup).
var jwtSettings =
    builder.Configuration
        .GetSection(JwtSettings.SectionName)
        .Get<JwtSettings>()
    ?? new JwtSettings();

// Validate in non-test production scenarios via Options at request time.
// For an eager startup check outside Testing add:
//   builder.Services.AddOptions<JwtSettings>()
//       .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
//       .ValidateDataAnnotations()
//       .ValidateOnStart();

var devCorsOrigins = new[]
{
    "http://localhost:5173",
    "https://localhost:5173"
};

// Database — only wire up SQL Server when a real connection string exists.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    if (!string.IsNullOrWhiteSpace(connectionString))
        options.UseSqlServer(connectionString);
    // When connectionString is null/empty the factory's ConfigureServices
    // replaces this registration with UseInMemoryDatabase before the host
    // starts, so no provider is needed here.
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

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan =
        TimeSpan.FromMinutes(15);
});

builder.Services
    .AddFluentValidationAutoValidation();

builder.Services
    .AddValidatorsFromAssemblyContaining<Program>();

builder.Services.Configure<
    Microsoft.AspNetCore.Http.Features.FormOptions>(options =>
{
    options.MultipartBodyLengthLimit =
        10 * 1024 * 1024;
});

// Authentication (JWT Bearer)
// NOTE: AddIdentity registers cookie auth schemes and can override default
// AuthenticationOptions. Configure Bearer AFTER Identity so [Authorize]
// uses JWT by default for API endpoints.
//
// TokenValidationParameters capture jwtSettings values here, but the
// factory's ConfigureAppConfiguration has already run by the time the
// host actually validates tokens on incoming requests, so the correct
// test values are in place when it matters.
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
        // Disable HTTPS requirement — integration tests run over plain
        // HTTP via TestServer; production uses HTTPS via Kestrel/reverse proxy.
        options.RequireHttpsMetadata = false;

        options.SaveToken = false;

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

                RoleClaimType = ClaimTypes.Role
            };

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
                        "Authorization header received for {Method} {Path}. " +
                        "StartsWithBearer={StartsWithBearer}, DoubleBearer={DoubleBearer}, Length={Length}",
                        context.Request.Method,
                        context.Request.Path,
                        authHeader.StartsWith("Bearer ",
                            StringComparison.OrdinalIgnoreCase),
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

builder.Services.AddCors(options =>
{
    options.AddPolicy("Frontend",
        policy =>
        {
            policy
                .WithOrigins(
                    "http://localhost:5173",
                    "https://localhost:5173")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory =
        context =>
        {
            var problemDetails =
                new ValidationProblemDetails(context.ModelState)
                {
                    Title = "Validation Failed",
                    Status = StatusCodes.Status400BadRequest
                };

            return new BadRequestObjectResult(problemDetails);
        };
});

builder.Services.AddProblemDetails(options =>
{
    options.IncludeExceptionDetails = (context, exception) =>
        builder.Environment.IsDevelopment();

    options.Map<NotFoundException>(exception =>
        new StatusCodeProblemDetails(StatusCodes.Status404NotFound)
        {
            Title = "Resource Not Found",
            Detail = exception.Message
        });

    options.Map<ConflictException>(exception =>
        new StatusCodeProblemDetails(StatusCodes.Status409Conflict)
        {
            Title = "Conflict",
            Detail = exception.Message
        });

    options.Map<Exception>(exception =>
    {
        Log.Error(exception, "Unhandled exception occurred");

        return new StatusCodeProblemDetails(
            StatusCodes.Status500InternalServerError)
        {
            Title = "Server Error",
            Detail = builder.Environment.IsDevelopment()
                ? exception.Message
                : "An unexpected error occurred."
        };
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("AuthPolicy", limiterOptions =>
    {
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.PermitLimit = 5;
        limiterOptions.QueueLimit = 0;
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerDocumentation();

builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

builder.WebHost.ConfigureKestrel(options =>
{
    options.AddServerHeader = false;
});

var app = builder.Build();

// Resolve JWT signing key per request so test-injected configuration
// is honored even when ConfigureAppConfiguration runs after Program.cs.
var jwtBearerOptions = app.Services
    .GetRequiredService<Microsoft.Extensions.Options.IOptionsMonitor<JwtBearerOptions>>()
    .Get(JwtBearerDefaults.AuthenticationScheme);

jwtBearerOptions.TokenValidationParameters.ValidIssuer =
    app.Services
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtSettings>>()
        .Value
        .Issuer;

jwtBearerOptions.TokenValidationParameters.ValidAudience =
    app.Services
        .GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtSettings>>()
        .Value
        .Audience;

jwtBearerOptions.TokenValidationParameters.IssuerSigningKeyResolver =
    (token, securityToken, kid, parameters) =>
    {
        var opts = app.Services
            .GetRequiredService<Microsoft.Extensions.Options.IOptions<JwtSettings>>()
            .Value;

        return new[]
        {
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(opts.Key))
        };
    };

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "GameStore API Docs";
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "GameStore API v1");
    });
}

app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(context =>
    {
        context.Response.StatusCode = 500;
        return Task.CompletedTask;
    });
});

app.UseProblemDetails();

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseSerilogRequestLogging(options =>
{
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
    {
        diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
        diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
        diagnosticContext.Set("CorrelationId", httpContext.Items["X-Correlation-Id"]);
    };
});

app.UseMiddleware<SecurityHeadersMiddleware>();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseCors("Frontend");

app.UseAuthentication();

app.UseAuthorization();

// Rate limiter BEFORE MapControllers so it sits ahead of endpoint dispatch.
app.UseRateLimiter();

app.MapControllers();

// Only run migrations and seed when a real database is configured.
// Skips automatically during testing because connectionString is null/empty
// (the factory never sets DefaultConnection to a real SQL Server string).
if (!string.IsNullOrWhiteSpace(connectionString))
{
    using var scope = app.Services.CreateAsyncScope();
    var services = scope.ServiceProvider;

    var context =
        services.GetRequiredService<ApplicationDbContext>();

    await context.Database.MigrateAsync();

    var roleManager =
        services.GetRequiredService<RoleManager<IdentityRole>>();

    var userManager =
        services.GetRequiredService<UserManager<ApplicationUser>>();

    await IdentitySeed.SeedRolesAsync(roleManager);
    await IdentitySeed.SeedAdminAsync(userManager);
    await ApplicationDbContextSeed.SeedAsync(context);
}

app.Run();

public partial class Program
{
}