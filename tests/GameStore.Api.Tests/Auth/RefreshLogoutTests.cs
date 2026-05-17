using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.DTOs.Auth;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Auth;

public class RefreshLogoutTests
    : IntegrationTestBase
{
    public RefreshLogoutTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Refresh_WithValidToken_ReturnsOk()
    {
        var loginResponse =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    Email = "admin@gamestore.com",
                    Password = "Admin123!"
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var refreshToken =
            ExtractRefreshToken(loginResponse);

        using var request =
            new HttpRequestMessage(
                HttpMethod.Post,
                "/api/auth/refresh");

        request.Headers.Add(
            "Cookie",
            $"refreshToken={refreshToken}");

        var response =
            await Client.SendAsync(request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var payload =
            await response.Content
                .ReadFromJsonAsync<AuthResponseDto>();

        payload!.AccessToken
            .Should()
            .NotBeNullOrWhiteSpace();
    }

    [Fact]
    public async Task Refresh_WithoutToken_ReturnsUnauthorized()
    {
        var response =
            await Client.PostAsync(
                "/api/auth/refresh",
                content: null);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);

        var payload =
            await response.Content
                .ReadFromJsonAsync<
                    Dictionary<string, string>>();

        payload!["message"]
            .Should()
            .Be("Missing refresh token.");
    }

    [Fact]
    public async Task Logout_WithValidToken_ClearsRefreshCookie()
    {
        var loginResponse =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    Email = "admin@gamestore.com",
                    Password = "Admin123!"
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var refreshToken =
            ExtractRefreshToken(loginResponse);

        var authPayload =
            await loginResponse.Content
                .ReadFromJsonAsync<AuthResponseDto>();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                authPayload!.AccessToken);

        using var request =
            new HttpRequestMessage(
                HttpMethod.Post,
                "/api/auth/logout");

        request.Headers.Add(
            "Cookie",
            $"refreshToken={refreshToken}");

        var response =
            await Client.SendAsync(request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        response.Headers
            .TryGetValues("Set-Cookie", out var cookies)
            .Should()
            .BeTrue();

        var logoutCookie =
            cookies!.FirstOrDefault(
                value => value.Contains(
                    "refreshToken=",
                    StringComparison.OrdinalIgnoreCase));

        logoutCookie
            .Should()
            .NotBeNull();

        logoutCookie!
            .ToLowerInvariant()
            .Should()
            .Contain("refreshtoken=")
            .And.Contain("expires=")
            .And.Contain("path=/api/auth");
    }

    private static string ExtractRefreshToken(
        HttpResponseMessage response)
    {
        response.Headers
            .TryGetValues("Set-Cookie", out var values)
            .Should()
            .BeTrue();

        var refreshCookie =
            values!.First(value =>
                value.StartsWith(
                    "refreshToken=",
                    StringComparison.OrdinalIgnoreCase));

        var tokenSegment =
            refreshCookie.Split(';')[0];

        return tokenSegment.Split('=')[1];
    }
}