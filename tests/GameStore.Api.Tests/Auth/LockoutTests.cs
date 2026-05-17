using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Auth;

public class LockoutTests
    : IntegrationTestBase
{
    public LockoutTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithTooManyFailures_ReturnsLockoutMessage()
    {
        var request = new
        {
            Email = "admin@gamestore.com",
            Password = "WrongPassword"
        };

        for (var attempt = 0; attempt < 4; attempt++)
        {
            var response =
                await Client.PostAsJsonAsync(
                    "/api/auth/login",
                    request);

            response.StatusCode
                .Should()
                .Be(HttpStatusCode.Unauthorized);
        }

        var lockoutResponse =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                request);

        lockoutResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);

        var payload =
            await lockoutResponse.Content
                .ReadFromJsonAsync<
                    Dictionary<string, string>>();

        payload!["message"]
            .Should()
            .Be("Account locked due to multiple failed attempts.");
    }
}