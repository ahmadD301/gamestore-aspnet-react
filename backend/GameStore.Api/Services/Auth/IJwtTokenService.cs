using GameStore.Api.DTOs.Auth;
using GameStore.Data.Entities;

namespace GameStore.Api.Services.Auth;

public interface IJwtTokenService
{
    Task<AuthResponseDto> CreateTokensAsync(
        ApplicationUser user);

    Task<AuthResponseDto?> RefreshTokenAsync(
        string refreshToken);

    Task RevokeRefreshTokenAsync(
        string refreshToken);
}