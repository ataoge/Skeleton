using Ataoge.Data;
using AutoMapper;
using System.Linq;

namespace Ataoge.AutoMapper
{
    public class ClaimExtensionValueResolver: IValueResolver<IClaimable, IExtensionByIndex, bool>
    {
        public bool Resolve(IClaimable source, IExtensionByIndex destination, bool destMember, ResolutionContext context)
        {
            if (source.Claims == null || source.Claims.Count() <= 0)
                return false;
            
            foreach(var claim in source.Claims)
            {
                destination[claim.Type] = claim.Value;
            }
           
            return true;
        }
    }
}