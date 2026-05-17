using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using GameStore.Api.Tests.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Tests.Auth;

public class RegisterTests
    : IntegrationTestBase
{
    public RegisterTests(
        TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Register_WithInvalidData_ReturnsBadRequestProblemDetails()
    {
        var request = new
        {
            Email = string.Empty,
            UserName = string.Empty,
            Password = "short"
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/auth/register",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        response.Content.Headers
            .ContentType
            ?.ToString()
            .Should()
            .Contain("application/problem+json");

        var problemDetails =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Title
            .Should()
            .Be("One or more validation errors occurred.");
    }

    [Fact]
    public async Task Register_WithDuplicateEmail_ReturnsConflictProblemDetails()
    {
        var request = new
        {
            Email = "admin@gamestore.com",
            UserName = "admin@gamestore.com",
            Password = "User123!"
        };

        var response =
            await Client.PostAsJsonAsync(
                "/api/auth/register",
                request);

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.Conflict);

        response.Content.Headers
            .ContentType
            ?.ToString()
            .Should()
            .Contain("application/problem+json");

        var problemDetails =
            await response.Content
                .ReadFromJsonAsync<ProblemDetails>();

        problemDetails!.Title
            .Should()
            .Be("Conflict");

        problemDetails.Detail
            .Should()
            .Contain("Email already exists");
    }
}
