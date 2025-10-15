namespace Workshop.Api.Middleware;

public class AuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationMiddleware> _logger;
    
    private static readonly HashSet<string> ExcludedPaths = new(StringComparer.OrdinalIgnoreCase)
    {
        "/swagger",
        "/health",
        "/api/health"
    };

    public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Skip authentication for excluded paths
        if (ShouldSkipAuthentication(context.Request.Path))
        {
            await _next(context);
            return;
        }

        // Extract headers
        if (!context.Request.Headers.TryGetValue("X-Username", out var username) || string.IsNullOrWhiteSpace(username))
        {
            _logger.LogWarning("Missing X-Username header");
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Missing X-Username header" });
            return;
        }

        if (!context.Request.Headers.TryGetValue("X-Role", out var role) || string.IsNullOrWhiteSpace(role))
        {
            _logger.LogWarning("Missing X-Role header for user {Username}", username.ToString());
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new { error = "Missing X-Role header" });
            return;
        }

        // Store user context
        var userContext = new Models.UserContext(username.ToString(), role.ToString());
        context.Items["User"] = userContext;

        _logger.LogInformation("Authenticated user {Username} with role {Role}", userContext.Username, userContext.Role);

        await _next(context);
    }

    private static bool ShouldSkipAuthentication(PathString path)
    {
        return ExcludedPaths.Any(excluded => path.StartsWithSegments(excluded, StringComparison.OrdinalIgnoreCase));
    }
}
