using Ataoge.EntityFrameworkCore.ModelConfiguration.Infrastructure;
using Ataoge.Modules;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

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