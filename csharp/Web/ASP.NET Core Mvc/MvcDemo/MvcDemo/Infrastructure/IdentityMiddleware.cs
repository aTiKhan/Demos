using System.Security.Claims;

namespace MvcDemo.WebSite.Infrastructure;

public class IdentityMiddleware
{
    public static int UserId = 10;

    private readonly RequestDelegate _next;

    public IdentityMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var claims = new[]
        {
            new Claim("UserId", UserId.ToString()),
            new Claim(ClaimTypes.Name, "User " + UserId),
            new Claim(ClaimTypes.NameIdentifier, UserId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Custom");
        context.User.AddIdentity(identity);

        await _next(context);
    }
}

public static class IdentityMiddlewareExtensions
{
    public static IApplicationBuilder UseFakeIdentity(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<IdentityMiddleware>();
    }
}
