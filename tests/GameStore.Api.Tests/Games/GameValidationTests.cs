using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.Tests.Helpers;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Games;

public class GameValidationTests
    : IntegrationTestBase
{
    public GameValidationTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task CreateGame_WithInvalidData_Returns400()
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
            Title = string.Empty,
            Description = string.Empty,
            Price = -1,
            ReleaseDateUtc =
                DateTime.UtcNow,
            GenreId = Guid.Empty
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/games",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        var contentType =
            response.Content.Headers
                .ContentType
                ?.ToString();

        contentType
            .Should()
            .Contain("application/problem+json");
    }
}
