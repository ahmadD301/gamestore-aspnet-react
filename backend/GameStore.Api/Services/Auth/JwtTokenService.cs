using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using GameStore.Api.Configuration;
using GameStore.Api.DTOs.Auth;
using GameStore.Data.Contexts;
using GameStore.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace GameStore.Api.Services.Auth;

public sealed class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public JwtTokenService(
        IOptions<JwtSettings> jwtOptions,
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _jwtSettings = jwtOptions.Value;
        _userManager = userManager;
        _context = context;
    }

    public async Task<AuthResponseDto> CreateTokensAsync(
        ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.UniqueName, user.UserName!),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        claims.AddRange(
            roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.Key));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var expires =
            DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials);

        var accessToken =
            new JwtSecurityTokenHandler().WriteToken(token);

        var refreshToken = GenerateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = Guid.NewGuid(),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAtUtc = DateTime.UtcNow.AddDays(7),
            CreatedAtUtc = DateTime.UtcNow
        };

        _context.RefreshTokens.Add(refreshTokenEntity);

        await _context.SaveChangesAsync();

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAtUtc = expires,
            Email = user.Email!,
            UserName = user.UserName!
        };
    }


    public async Task<AuthResponseDto?> RefreshTokenAsync(
        string refreshToken)
    {
        var existingToken =
            await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r =>
                    r.Token == refreshToken);

        if (existingToken is null)
        {
            return null;
        }

        if (existingToken.IsRevoked)
        {
            return null;
        }

        if (existingToken.ExpiresAtUtc < DateTime.UtcNow)
        {
            return null;
        }

        existingToken.IsRevoked = true;

        existingToken.RevokedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return await CreateTokensAsync(existingToken.User);
    }

    public async Task RevokeRefreshTokenAsync(
        string refreshToken)
    {
        var existingToken =
            await _context.RefreshTokens
                .FirstOrDefaultAsync(r =>
                    r.Token == refreshToken);

        if (existingToken is null)
        {
            return;
        }

        existingToken.IsRevoked = true;

        existingToken.RevokedAtUtc = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }
}