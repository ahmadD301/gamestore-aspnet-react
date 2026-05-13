using Microsoft.AspNetCore.Identity;

namespace GameStore.Data.Entities;

public sealed class ApplicationUser : IdentityUser
{
    public DateTime CreatedAtUtc { get; set; }
        = DateTime.UtcNow;

    public ICollection<Review> Reviews { get; set; }
        = new List<Review>();

    public ICollection<WishlistItem> WishlistItems { get; set; }
        = new List<WishlistItem>();

    public ICollection<RefreshToken> RefreshTokens { get; set; }
        = new List<RefreshToken>();
}