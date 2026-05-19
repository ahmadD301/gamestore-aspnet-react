namespace GameStore.Api.DTOs.Games;

public sealed class CreateGameRequestDto
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public decimal Price { get; set; }

    public string CoverImageUrl { get; set; }
        = string.Empty;

    public DateTime ReleaseDateUtc { get; set; }

    public Guid GenreId { get; set; }
}