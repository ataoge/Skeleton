using System;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Modules
{
    public interface IModule
    {
        IModuleManager ModuleManager {get;}
        
        void ConfiguringService(IServiceCollection services);
        
        void ConfiguredService(IServiceCollection services);

        void Initialize(IServiceProvider serviceProvider);

        void Shutdown();
    }
}