using Workshop.Api.Models;

namespace Workshop.Api.Extensions;

public static class HttpContextExtensions
{
    public static UserContext? GetCurrentUser(this HttpContext context)
    {
        return context.Items.TryGetValue("User", out var user) ? user as UserContext : null;
    }

    public static string? GetCurrentUsername(this HttpContext context)
    {
        return context.GetCurrentUser()?.Username;
    }

    public static string? GetCurrentUserRole(this HttpContext context)
    {
        return context.GetCurrentUser()?.Role;
    }

    public static bool IsAdmin(this HttpContext context)
    {
        return context.GetCurrentUserRole()?.Equals(Roles.Admin, StringComparison.OrdinalIgnoreCase) ?? false;
    }

    public static bool IsEmployee(this HttpContext context)
    {
        return context.GetCurrentUserRole()?.Equals(Roles.Employee, StringComparison.OrdinalIgnoreCase) ?? false;
    }
}
