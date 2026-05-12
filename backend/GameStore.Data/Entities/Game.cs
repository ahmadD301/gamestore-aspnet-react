namespace GameStore.Data.Entities;

public sealed class Game : BaseEntity
{
    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public string CoverImageUrl { get; set; } = string.Empty;

    public DateTime ReleaseDateUtc { get; set; }

    // FK
    public Guid GenreId { get; set; }

    // Navigation
    public Genre Genre { get; set; } = null!;

    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();

    public ICollection<WishlistItem> WishlistItems { get; set; }
        = new List<WishlistItem>();
}