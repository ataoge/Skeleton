using Ataoge.Configuration;
using Ataoge.Modules;

namespace Ataoge
{
    public class BootStrapperOptions
    {
        public IStartupConfiguration StartupConfiguration {get; set;}

        public IModuleManager ModuleManager {get; set;}
    }
}