using GameStore.Api.DTOs.Auth;
using GameStore.Data.Entities;

namespace GameStore.Api.Services.Auth;

public interface IJwtTokenService
{
    Task<TokenResult> CreateTokensAsync(
        ApplicationUser user);

    Task<TokenResult?> RefreshTokenAsync(
        string refreshToken);

    Task RevokeRefreshTokenAsync(
        string refreshToken);
}