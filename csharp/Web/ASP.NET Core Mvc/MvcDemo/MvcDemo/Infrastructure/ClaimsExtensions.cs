using System.Security.Claims;

namespace MvcDemo.WebSite.Infrastructure
{
    public static class ClaimsExtensions
    {
        public static int GetUserId(this ClaimsPrincipal principal)
        {
            return int.Parse(principal.Claims.FirstOrDefault(x => x.Type == "UserId")!.Value);
        }
    }
}
