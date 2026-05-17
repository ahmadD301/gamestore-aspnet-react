using GameStore.Data.Entities;

namespace GameStore.Api.Services.Auth;

public sealed class TokenResult
{
    public string AccessToken
    { get; set; } = string.Empty;

    public string RefreshToken
    { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc
    { get; set; }

    public ApplicationUser User
    { get; set; } = null!;
}