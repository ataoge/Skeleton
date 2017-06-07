using System;
using Ataoge.Modules;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Ataoge.Configuration
{
    public class ConfigBootStrapperOptions : IConfigureOptions<BootStrapperOptions>
    {
        public ConfigBootStrapperOptions(IServiceProvider serviceProvider)
        {
            this._sp = serviceProvider;
        }

        private readonly IServiceProvider _sp;

        public void Configure(BootStrapperOptions options)
        {
            options.StartupConfiguration = this._sp.GetRequiredService<IStartupConfiguration>();
            options.StartupConfiguration.Initialize();
            options.ModuleManager = this._sp.GetRequiredService<IModuleManager>();
        }
    }
}