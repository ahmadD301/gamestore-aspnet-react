namespace GameStore.Api.DTOs.Games;

public sealed class GameResponseDto
{
    public Guid Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; }
        = string.Empty;

    public decimal Price { get; set; }

    public DateTime ReleaseDateUtc { get; set; }

    public string Genre { get; set; } = string.Empty;

    public DateTime CreatedAtUtc { get; set; }
}