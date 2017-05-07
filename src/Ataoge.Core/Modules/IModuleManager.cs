using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Modules
{
    public interface IModuleManager
    {
        ModuleInfo StartupModule { get; }

        IReadOnlyList<ModuleInfo> Modules { get; }

        void Initialize(Type startupModule);

        void ConfigModules(IServiceCollection services);

        void StartModules(IServiceProvider serviceProvider);

        void ShutdownModules();
    }
}