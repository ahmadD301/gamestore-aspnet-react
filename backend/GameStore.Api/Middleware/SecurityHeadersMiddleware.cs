namespace GameStore.Api.Middleware;

public class SecurityHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public SecurityHeadersMiddleware(
        RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context)
    {
        context.Response.Headers[
            "X-Content-Type-Options"
        ] = "nosniff";

        context.Response.Headers[
            "X-Frame-Options"
        ] = "DENY";

        context.Response.Headers[
            "Referrer-Policy"
        ] = "strict-origin-when-cross-origin";

        context.Response.Headers[
            "X-XSS-Protection"
        ] = "0";

        context.Response.Headers[
            "Permissions-Policy"
        ] =
            "camera=(), microphone=(), geolocation=()";

        context.Response.Headers[
            "Content-Security-Policy"
        ] =
            "default-src 'self'; " +
            "img-src 'self' data: https:; " +
            "style-src 'self' 'unsafe-inline'; " +
            "script-src 'self';";

        await _next(context);
    }
}