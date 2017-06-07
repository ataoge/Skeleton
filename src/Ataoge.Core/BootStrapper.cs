using System;
using Ataoge.Configuration;
using Ataoge.Modules;
using Microsoft.Extensions.Options;

namespace Ataoge
{
    public class BootStrapper : IDisposable
    {
        public BootStrapper(IOptions<BootStrapperOptions> options)
        {
            this.Options = options.Value;
        }

        public BootStrapperOptions Options {get; private set;}


        public IStartupConfiguration StartupConfiguration { get{ return Options.StartupConfiguration;} }
        public IModuleManager ModuleManager {get {return Options.ModuleManager;}}


        public void Initialize(IServiceProvider serviceProvider)
        {
            //ModuleManager.ModuleConfigurations = StartupConfiguration.Modules;
            ModuleManager.StartModules(serviceProvider);
        }

         /// <summary>
        /// Is this object disposed before?
        /// </summary>
        protected bool IsDisposed;

        public void Dispose()
        {
             if (IsDisposed)
            {
                return;
            }

            IsDisposed = true;

            ModuleManager?.ShutdownModules();
        }
    }
}