namespace GameStore.Data.Entities;

public sealed class WishlistItem : BaseEntity
{
    // FK
    public Guid GameId { get; set; }

    public string UserId { get; set; } = string.Empty;

    // Navigation
    public Game Game { get; set; } = null!;

    public ApplicationUser User { get; set; } = null!;
}