using System.Security.Claims;
using System.Threading;
using Ataoge.Dependency;

namespace Ataoge.Runtime.Session
{
    public class DefaultPrincipalAccessor : IPrincipalAccessor, ISingletonDependency
    {
        public virtual ClaimsPrincipal Principal => null;


        public static DefaultPrincipalAccessor Instance => new DefaultPrincipalAccessor();
    }
}