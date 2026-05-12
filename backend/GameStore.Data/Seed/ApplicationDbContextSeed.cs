using GameStore.Data.Contexts;
using GameStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Data.Seed;

public static class ApplicationDbContextSeed
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (await context.Genres.AnyAsync())
        {
            return;
        }

        var actionGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Action",
            CreatedAtUtc = DateTime.UtcNow
        };

        var rpgGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "RPG",
            CreatedAtUtc = DateTime.UtcNow
        };

        var strategyGenre = new Genre
        {
            Id = Guid.NewGuid(),
            Name = "Strategy",
            CreatedAtUtc = DateTime.UtcNow
        };

        await context.Genres.AddRangeAsync(
            actionGenre,
            rpgGenre,
            strategyGenre);

        var games = new List<Game>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Cyber Adventure",
                Description = "Futuristic action adventure game.",
                Price = 59.99m,
                CoverImageUrl =
                    "https://images.igdb.com/example1.jpg",
                ReleaseDateUtc = new DateTime(2024, 5, 10),
                GenreId = actionGenre.Id,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Kingdom Legends",
                Description = "Fantasy RPG with open world exploration.",
                Price = 69.99m,
                CoverImageUrl =
                    "https://images.igdb.com/example2.jpg",
                ReleaseDateUtc = new DateTime(2023, 11, 18),
                GenreId = rpgGenre.Id,
                CreatedAtUtc = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Empire Tactics",
                Description = "Turn-based strategy warfare simulator.",
                Price = 49.99m,
                CoverImageUrl =
                    "https://images.igdb.com/example3.jpg",
                ReleaseDateUtc = new DateTime(2022, 8, 1),
                GenreId = strategyGenre.Id,
                CreatedAtUtc = DateTime.UtcNow
            }
        };

        await context.Games.AddRangeAsync(games);

        await context.SaveChangesAsync();
    }
}