using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.Tests.Infrastructure;

namespace GameStore.Api.Tests.Auth;

public class LoginTests
    : IntegrationTestBase
{
    public LoginTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsOk()
    {
        var request = new
        {
            Email =
                "admin@gamestore.com",

            Password =
                "Admin123!"
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var request = new
        {
            Email =
                "admin@gamestore.com",

            Password =
                "WrongPassword"
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/auth/login",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Unauthorized);
    }
}