using System.Security.Claims;
using Tirajii.Data.Models;

namespace Tirajii.Infrastructure.Extensions
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Id(this ClaimsPrincipal user)
        {
            var userId = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (userId is null) throw new ArgumentException("Invalid User!");
            return userId.ToString();
        }

        public static bool IsAdmin(this ClaimsPrincipal user)
            => user.IsInRole("Administrator");
    }
}
