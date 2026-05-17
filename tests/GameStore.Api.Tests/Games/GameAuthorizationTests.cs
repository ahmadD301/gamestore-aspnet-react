using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.DTOs.Auth;
using GameStore.Api.Tests.Helpers;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Games;

public class GameAuthorizationTests
    : IntegrationTestBase
{
    public GameAuthorizationTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateGame_WithoutAuth_Returns401()
    {
        var request = new
        {
            Title = "New Game",
            Description =
                "Test description",
            Price = 59.99,
            GenreId = Guid.NewGuid()
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/games",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task CreateGame_AsAdmin_Returns201()
    {
        var token =
            await AuthHelper
                .LoginAsAdminAsync(
                    Client);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var request = new
        {
            Title = "New Game",
            Description =
                "Long enough description",
            Price = 49.99,
            ReleaseDate =
                DateTime.UtcNow,
            GenreId =
                await GetGenreId()
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/games",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task CreateGame_AsNonAdmin_Returns403()
    {
        var token =
            await GetNonAdminTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var request = new
        {
            Title = "Non-admin game",
            Description =
                "Long enough description",
            Price = 19.99,
            ReleaseDateUtc =
                DateTime.UtcNow,
            GenreId =
                await GetGenreId()
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/games",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UpdateGame_WithoutAuth_Returns401()
    {
        var response =
            await Client.PutAsJsonAsync(
                $"/api/games/{Guid.NewGuid()}",
                new
                {
                    Title = "Update",
                    Description = "Updated description",
                    Price = 25.99,
                    ReleaseDateUtc = DateTime.UtcNow,
                    GenreId = Guid.NewGuid()
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task DeleteGame_WithoutAuth_Returns401()
    {
        var response =
            await Client.DeleteAsync(
                $"/api/games/{Guid.NewGuid()}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task UpdateGame_AsNonAdmin_Returns403()
    {
        var token =
            await GetNonAdminTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await Client.PutAsJsonAsync(
                $"/api/games/{Guid.NewGuid()}",
                new
                {
                    Title = "Update",
                    Description = "Updated description",
                    Price = 25.99,
                    ReleaseDateUtc = DateTime.UtcNow,
                    GenreId = Guid.NewGuid()
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task DeleteGame_AsNonAdmin_Returns403()
    {
        var token =
            await GetNonAdminTokenAsync();

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var response =
            await Client.DeleteAsync(
                $"/api/games/{Guid.NewGuid()}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Forbidden);
    }

    private async Task<Guid>
    GetGenreId()
    {
        var genres =
            await Client.GetFromJsonAsync<
                List<GenreResponse>>(
                "/api/genres");

        return genres!.First().Id;
    }

    private async Task<string> GetNonAdminTokenAsync()
    {
        var userEmail =
            $"user-{Guid.NewGuid():N}@gamestore.com";

        var registerRequest = new
        {
            Email = userEmail,
            UserName = userEmail,
            Password = "User123!"
        };

        var registerResponse =
            await Client.PostAsJsonAsync(
                "/api/auth/register",
                registerRequest);

        registerResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var loginResponse =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                new
                {
                    Email = userEmail,
                    Password = "User123!"
                });

        loginResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var payload =
            await loginResponse.Content
                .ReadFromJsonAsync<AuthResponseDto>();

        return payload!.AccessToken;
    }

    private class GenreResponse
    {
        public Guid Id { get; set; }
    }
}