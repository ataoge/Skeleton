using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace Ataoge.AspNetCore.Security
{
    public class ClaimsTransformer : IClaimsTransformer
    {


        public Task<ClaimsPrincipal> TransformAsync(ClaimsTransformationContext context)
        {
            var principal = context.Principal;
            
            foreach (var claim in principal.Claims)
            {
                if (claim.Type == "role")
                {
                     ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Role, claim.Value));
                }

                if (claim.Type == "sub")
                {
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.NameIdentifier, claim.Value));
                }

                if (claim.Type == "name")
                {
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Name, claim.Value));
                }

                if (claim.Type == "email")
                {
                    ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.Email, claim.Value));
                }
            }
           
            return Task.FromResult(principal);
      
        }
    }
}