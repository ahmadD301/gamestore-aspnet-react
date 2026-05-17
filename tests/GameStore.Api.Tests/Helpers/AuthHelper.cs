using System.Net.Http.Json;

namespace GameStore.Api.Tests.Helpers;

public static class AuthHelper
{
    public static async Task<string>
    LoginAsAdminAsync(
        HttpClient client)
    {
        var request = new
        {
            Email =
                "admin@gamestore.com",

            Password =
                "Admin123!"
        };

        var response =
            await client.PostAsJsonAsync(
                "/api/auth/login",
                request);

        response.EnsureSuccessStatusCode();

        var payload =
            await response.Content
                .ReadFromJsonAsync<
                    LoginResponse>();

        return payload!.AccessToken;
    }

    private class LoginResponse
    {
        public string AccessToken
        {
            get;
            set;
        } = default!;
    }
}