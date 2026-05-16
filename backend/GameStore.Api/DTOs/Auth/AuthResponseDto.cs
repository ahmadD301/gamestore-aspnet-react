namespace GameStore.Api.DTOs.Auth;

public sealed class AuthResponseDto
{
    public string AccessToken
    { get; set; } = string.Empty;

    public DateTime ExpiresAtUtc
    { get; set; }

    public string Email
    { get; set; } = string.Empty;

    public string UserName
    { get; set; } = string.Empty;

    public List<string> Roles
    { get; set; } = [];
}