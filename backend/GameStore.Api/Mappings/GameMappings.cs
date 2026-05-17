using GameStore.Api.DTOs.Games;
using GameStore.Data.Entities;

namespace GameStore.Api.Mappings;

public static class GameMappings
{
    public static GameResponseDto ToDto(
        this Game game)
    {
        return new GameResponseDto
        {
            Id = game.Id,
            Title = game.Title,
            Description = game.Description,
            Price = game.Price,
            ReleaseDateUtc = game.ReleaseDateUtc,
            Genre = game.Genre.Name,
            CreatedAtUtc = game.CreatedAtUtc,
            UpdatedAtUtc = game.UpdatedAtUtc,
            CreatedBy = game.CreatedBy,
            UpdatedBy = game.UpdatedBy,
            GenreId = game.GenreId
        };
    }
}