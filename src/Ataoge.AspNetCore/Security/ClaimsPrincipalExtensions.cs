using System.Linq;
using System.Security.Principal;

namespace System.Security.Claims
{
    public static class ClaimsPrincipalExtensions
    {

        public static string GetValue(this ClaimsPrincipal principal,  string name)
        {
            return principal.Claims.FirstOrDefault(c => c.Type==name)?.Value;
        }
       
        public static string GetDisplayName(this ClaimsPrincipal principal)
        {
            var claim =principal.Claims.FirstOrDefault(c => c.Type=="preferred_username");
            if (claim != null)
                return claim.Value;
            return principal.Identity.Name;
        }

        public static string GetAvatarImageUrl(this ClaimsPrincipal principal)
        {
            return GetValue(principal, "picture");
           
        }

        public static string GetUserId(this ClaimsPrincipal principal)
        {
            return GetValue(principal, ClaimTypes.NameIdentifier);
        }
    }
}