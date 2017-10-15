using Ataoge.AspNetCore.Runtime.Session;
using Ataoge.Data.Metadata;
using Ataoge.Data.Metadata.Internal;
using Ataoge.Modules;
using Ataoge.Runtime.Session;
using Ataoge.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ataoge.AspNetCore
{
    [DependsOn(typeof(KernelModule))]
    public class AspNetCoreModule : ModuleBase
    {
        protected override void OnConfiguringService(IServiceCollection services)
        {
            services.TryAddSingleton<IPrincipalAccessor, AspNetCorePrincipalAccessor>();
            services.TryAddSingleton<IUrlHelper, AtaogeUrlHelper>();
            services.TryAddTransient<IServiceContext, AspNetCoreServiceContext>();
            
            services.TryAddSingleton<IViewModelManager, ViewModelManager>();
        }


        protected override void OnConfiguredService(IServiceCollection services)
        {
             //services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
             //services.TryAddSingleton<IActionContextAccessor, ActionContextAccessor>();
        }
        
    }
}