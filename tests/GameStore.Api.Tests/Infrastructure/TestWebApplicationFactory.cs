using GameStore.Api.Tests.Fixtures;
using GameStore.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Tests.Infrastructure;

public class TestWebApplicationFactory
    : WebApplicationFactory<Program>
{
    private readonly string _databaseName =
        $"GameStoreTestDb-{Guid.NewGuid():N}";

    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        // Inject JWT + connection-string config BEFORE Program.cs runs.
        // ConfigureAppConfiguration callbacks execute first in the pipeline,
        // so these values are visible when Program.cs reads JwtSettings.
        builder.ConfigureAppConfiguration((context, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["JwtSettings:Issuer"]   = "GameStore.Tests",
                ["JwtSettings:Audience"] = "GameStore.Tests",

                // Must be ≥ 32 chars for HMAC-SHA256.
                ["JwtSettings:Key"]      = "testing-only-secret-key-32chars!",
                ["JwtSettings:ExpiryMinutes"] = "15",

                // Dummy value so the non-Testing connection-string guard
                // in Program.cs never throws if the isTesting guard is missed.
                ["ConnectionStrings:DefaultConnection"] = "Testing"
            });
        });

        // ConfigureServices MUST be a synchronous Action<IServiceCollection>.
        // The framework ignores any returned Task, so async lambdas silently
        // fire-and-forget — seeding would never complete.
        builder.ConfigureServices(services =>
        {
            // ── 1. Swap SQL Server DbContext for InMemory ──────────────────
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
                services.Remove(descriptor);

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase(_databaseName));

            // ── 2. Seed data ───────────────────────────────────────────────
            // Build from the ROOT service provider so TestDataSeeder.SeedAsync
            // can call CreateScope() on it and get a fresh, correctly-wired
            // scope with the InMemory DbContext.
            //
            // Bug that was here before: the factory built a scope first, then
            // passed the SCOPED provider to TestDataSeeder, which called
            // CreateScope() again — producing a nested scope whose DbContext
            // instance was different from the one EnsureCreated() ran on.
            // The seeded data was written to an orphaned context and the tests
            // saw an empty database.
            var rootProvider = services.BuildServiceProvider();

            // EnsureCreated on the root provider's own scope so the InMemory
            // schema is initialised before TestDataSeeder touches the db.
            using (var initScope = rootProvider.CreateScope())
            {
                var db = initScope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();
            }

            // TestDataSeeder creates its own internal scope from rootProvider,
            // so it shares the same InMemory database instance.
            TestDataSeeder
                .SeedAsync(rootProvider)
                .GetAwaiter()
                .GetResult();
        });
    }
}