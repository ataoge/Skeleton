using Ataoge.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Modules
{
    public class KernelModule : Module
    {
        public KernelModule()
        {

        }


        protected override void OnConfigureService(IServiceCollection services)
        {
            services.AddSingleton<IStartupConfiguration>(sp => new StartupConfiguration(sp));
        }
    }
}