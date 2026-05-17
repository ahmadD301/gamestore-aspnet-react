using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.Tests.Helpers;
using GameStore.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Tests.Games;

public class GameNotFoundTests
    : IntegrationTestBase
{
    public GameNotFoundTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetGame_WithUnknownId_ReturnsProblemDetails()
    {
        var response =
            await Client.GetAsync(
                $"/api/games/{Guid.NewGuid()}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);

        var problemDetails =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Title
            .Should()
            .Be("Resource Not Found");
    }

    [Fact]
    public async Task UpdateGame_WithUnknownId_ReturnsProblemDetails()
    {
        await AuthenticateAdminAsync();

        var response =
            await Client.PutAsJsonAsync(
                $"/api/games/{Guid.NewGuid()}",
                new
                {
                    Title = "Updated",
                    Description = "Updated description",
                    Price = 49.99,
                    ReleaseDateUtc = DateTime.UtcNow,
                    GenreId = await GetGenreIdAsync()
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);

        var problemDetails =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Title
            .Should()
            .Be("Resource Not Found");
    }

    [Fact]
    public async Task DeleteGame_WithUnknownId_ReturnsProblemDetails()
    {
        await AuthenticateAdminAsync();

        var response =
            await Client.DeleteAsync(
                $"/api/games/{Guid.NewGuid()}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);

        var problemDetails =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Title
            .Should()
            .Be("Resource Not Found");
    }

    private async Task AuthenticateAdminAsync()
    {
        var token =
            await AuthHelper
                .LoginAsAdminAsync(
                    Client);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);
    }

    private async Task<Guid> GetGenreIdAsync()
    {
        var genres =
            await Client.GetFromJsonAsync<
                List<GenreResponseDto>>(
                "/api/genres");

        return genres!.First().Id;
    }

    private class GenreResponseDto
    {
        public Guid Id { get; set; }
    }
}
