namespace GameStore.Api.DTOs.Auth;

public sealed class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = string.Empty;
}