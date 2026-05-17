using GameStore.Api.Tests.Fixtures;
using GameStore.Data;
using GameStore.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Tests.Infrastructure;

public class TestWebApplicationFactory
    : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(
        IWebHostBuilder builder)
    {
        builder.UseEnvironment(
            "Testing");

        builder.ConfigureServices(
            async services =>
            {
                var descriptor =
                    services.SingleOrDefault(
                        d =>
                            d.ServiceType ==
                            typeof(
                                DbContextOptions<
                                    ApplicationDbContext>));

                if (descriptor != null)
                {
                    services.Remove(
                        descriptor);
                }

                services.AddDbContext<
                    ApplicationDbContext>(
                    options =>
                    {
                        options.UseInMemoryDatabase(
                            "GameStoreTestDb");
                    });

                var serviceProvider =
                    services.BuildServiceProvider();

                using var scope =
                    serviceProvider.CreateScope();

                var scopedServices =
                    scope.ServiceProvider;

                await TestDataSeeder.SeedAsync(
                    scopedServices);

                var db =
                    scope.ServiceProvider
                        .GetRequiredService<
                            Data.Contexts.ApplicationDbContext>();

                db.Database.EnsureCreated();
            });
    }
}