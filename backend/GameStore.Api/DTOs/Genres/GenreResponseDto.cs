namespace GameStore.Api.DTOs.Genres;

public sealed class GenreResponseDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
}