using System.Security.Claims;

namespace TheBugInspector.Helpers.Extensions
{
    public static class ClaimsPrinicpleExtensions
    {
        public static string? GetUserId(this ClaimsPrincipal principal)
        {
            return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
