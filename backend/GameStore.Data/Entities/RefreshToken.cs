namespace GameStore.Data.Entities;

public sealed class RefreshToken : BaseEntity
{
    public string Token { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc { get; set; }

    public bool IsRevoked { get; set; }

    public string UserId { get; set; } = string.Empty;

    public ApplicationUser User { get; set; } = null!;
}