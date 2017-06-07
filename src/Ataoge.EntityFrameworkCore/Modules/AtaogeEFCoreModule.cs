using Ataoge.Modules;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EntityFrameworkCore.Modules
{
    [DependsOn(typeof(KernelModule))]
    public class AtaogeEFCoreModule : ModuleBase
    {
        protected override void OnConfiguringService(IServiceCollection services)
        {
            
        }
    }
}