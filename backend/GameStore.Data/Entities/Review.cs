namespace GameStore.Data.Entities;

public sealed class Review : BaseEntity
{
    public int Rating { get; set; }

    public string Comment { get; set; } = string.Empty;

    // FK
    public Guid GameId { get; set; }

    public string UserId { get; set; } = string.Empty;

    // Navigation
    public Game Game { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}