using GameStore.Data.Contexts;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace GameStore.Api.Tests.Fixtures;

public static class TestDataSeeder
{
    public static async Task SeedAsync(
        IServiceProvider services)
    {
        using var scope =
            services.CreateScope();

        var db =
            scope.ServiceProvider
                .GetRequiredService<
                    ApplicationDbContext>();

        var userManager =
            scope.ServiceProvider
                .GetRequiredService<
                    UserManager<ApplicationUser>>();

        var roleManager =
            scope.ServiceProvider
                .GetRequiredService<
                    RoleManager<IdentityRole>>();

        if (!await roleManager.RoleExistsAsync(
            "Admin"))
        {
            await roleManager.CreateAsync(
                new IdentityRole(
                    "Admin"));
        }

        var admin =
            await userManager
                .FindByEmailAsync(
                    "admin@gamestore.com");

        if (admin == null)
        {
            admin =
                new ApplicationUser
                {
                    UserName =
                        "admin@gamestore.com",

                    Email =
                        "admin@gamestore.com"
                };

            await userManager.CreateAsync(
                admin,
                "Admin123!");

            await userManager.AddToRoleAsync(
                admin,
                "Admin");
        }

        if (!db.Genres.Any())
        {
            db.Genres.Add(
                new Genre
                {
                    Id = Guid.NewGuid(),
                    Name = "RPG"
                });

            await db.SaveChangesAsync();
        }
    }
}