using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.DTOs.Common;
using GameStore.Api.DTOs.Games;
using GameStore.Api.Tests.Helpers;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Games;

public class GamePaginationTests
    : IntegrationTestBase
{
    public GamePaginationTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task GetGames_WithPageSize_ReturnsExpectedMetadata()
    {
        var prefix =
            $"Pagination-{Guid.NewGuid():N}";

        await CreateGamesAsync(prefix, 3);

        var response =
            await Client.GetAsync(
                $"/api/games?search={prefix}&page=1&pageSize=2");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var payload =
            await response.Content
                .ReadFromJsonAsync<
                    PagedResultDto<GameResponseDto>>();

        payload.Should().NotBeNull();

        payload!.Items
            .Should()
            .HaveCount(2);

        payload.Page
            .Should()
            .Be(1);

        payload.PageSize
            .Should()
            .Be(2);

        payload.TotalCount
            .Should()
            .Be(3);

        payload.TotalPages
            .Should()
            .Be(2);
    }

    [Fact]
    public async Task GetGames_WithEmptyPage_ReturnsNoItems()
    {
        var prefix =
            $"Pagination-{Guid.NewGuid():N}";

        await CreateGamesAsync(prefix, 3);

        var response =
            await Client.GetAsync(
                $"/api/games?search={prefix}&page=3&pageSize=2");

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var payload =
            await response.Content
                .ReadFromJsonAsync<
                    PagedResultDto<GameResponseDto>>();

        payload.Should().NotBeNull();

        payload!.Items
            .Should()
            .BeEmpty();

        payload.Page
            .Should()
            .Be(3);

        payload.PageSize
            .Should()
            .Be(2);

        payload.TotalCount
            .Should()
            .Be(3);

        payload.TotalPages
            .Should()
            .Be(2);
    }

    private async Task CreateGamesAsync(
        string prefix,
        int count)
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

        for (var index = 0; index < count; index++)
        {
            var request = new
            {
                Title = $"{prefix}-{index + 1}",
                Description =
                    "Long enough description",
                Price = 29.99 + index,
                ReleaseDateUtc =
                    DateTime.UtcNow.AddDays(-index),
                GenreId = genreId
            };

            var response =
                await Client.PostAsJsonAsync(
                    "/api/games",
                    request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Created);
        }
    }

    private async Task<Guid> GetGenreIdAsync()
    {
        var genres =
            await Client.GetFromJsonAsync<
                List<GenreResponse>>(
                "/api/genres");

        return genres!.First().Id;
    }

    private class GenreResponse
    {
        public Guid Id { get; set; }
    }
}
