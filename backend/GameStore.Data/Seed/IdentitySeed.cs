using GameStore.Data.Constants;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Seed;

public static class IdentitySeed
{
    public static async Task SeedRolesAsync(
        RoleManager<IdentityRole> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(Roles.Admin))
        {
            await roleManager.CreateAsync(
                new IdentityRole(Roles.Admin));
        }

        if (!await roleManager.RoleExistsAsync(Roles.User))
        {
            await roleManager.CreateAsync(
                new IdentityRole(Roles.User));
        }
    }

    public static async Task SeedAdminAsync(
        UserManager<ApplicationUser> userManager)
    {
        const string adminEmail = "admin@gamestore.com";

        var existingAdmin =
            await userManager.FindByEmailAsync(adminEmail);

        if (existingAdmin is not null)
        {
            return;
        }

        var adminUser = new ApplicationUser
        {
            Email = adminEmail,
            UserName = "admin",
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(
            adminUser,
            "Admin123!");

        if (!result.Succeeded)
        {
            throw new Exception(
                "Failed to create admin user.");
        }

        await userManager.AddToRoleAsync(
            adminUser,
            Roles.Admin);
    }
}