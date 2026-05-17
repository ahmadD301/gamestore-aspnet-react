using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.DTOs.Games;
using GameStore.Api.Tests.Helpers;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Games;

public class GameCrudTests
    : IntegrationTestBase
{
    public GameCrudTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetGame_ById_ReturnsGame()
    {
        var createdGame =
            await CreateGameAsync();

        var response =
            await Client.GetAsync(
                $"/api/games/{createdGame.Id}");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var payload =
            await response.Content
                .ReadFromJsonAsync<GameResponseDto>();

        payload!.Id
            .Should()
            .Be(createdGame.Id);

        payload.Title
            .Should()
            .Be(createdGame.Title);
    }

    [Fact]
    public async Task UpdateGame_WithValidData_ReturnsNoContent()
    {
        var createdGame =
            await CreateGameAsync();

        var token =
            await AuthHelper
                .LoginAsAdminAsync(
                    Client);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var updateRequest = new
        {
            Title = "Updated title",
            Description = "Updated description",
            Price = 39.99,
            ReleaseDateUtc = DateTime.UtcNow,
            GenreId = createdGame.GenreId
        };

        var updateResponse =
            await Client.PutAsJsonAsync(
                $"/api/games/{createdGame.Id}",
                updateRequest);

        updateResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var getResponse =
            await Client.GetAsync(
                $"/api/games/{createdGame.Id}");

        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var updatedGame =
            await getResponse.Content
                .ReadFromJsonAsync<GameResponseDto>();

        updatedGame!.Title
            .Should()
            .Be("Updated title");

        updatedGame.Description
            .Should()
            .Be("Updated description");
    }

    [Fact]
    public async Task DeleteGame_WithValidData_ReturnsNoContent()
    {
        var createdGame =
            await CreateGameAsync();

        var token =
            await AuthHelper
                .LoginAsAdminAsync(
                    Client);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var deleteResponse =
            await Client.DeleteAsync(
                $"/api/games/{createdGame.Id}");

        deleteResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NoContent);

        var getResponse =
            await Client.GetAsync(
                $"/api/games/{createdGame.Id}");

        getResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    private async Task<GameResponseDto> CreateGameAsync()
    {
        var token =
            await AuthHelper
                .LoginAsAdminAsync(
                    Client);

        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        var genreId =
            await GetGenreIdAsync();

        var request = new
        {
            Title = $"Game-{Guid.NewGuid():N}",
            Description = "Long enough description",
            Price = 19.99,
            ReleaseDateUtc = DateTime.UtcNow,
            GenreId = genreId
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/games",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Created);

        var payload =
            await response.Content
                .ReadFromJsonAsync<GameResponseDto>();

        return payload!;
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
