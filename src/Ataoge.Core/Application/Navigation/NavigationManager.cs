using System.Collections.Generic;
using Ataoge.Dependency;

namespace Ataoge.Application.Navigation
{
    internal class NavigationManager : INavigationManager, ISingletonDependency
    {
        public IDictionary<string, MenuDefinition> Menus { get; private set; }

        public MenuDefinition MainMenu
        {
            get { return Menus["MainMenu"]; }
        }

        public NavigationManager()
        {
            //_iocResolver = iocResolver;
            //_configuration = configuration;

            Menus = new Dictionary<string, MenuDefinition>
                    {
                        {"MainMenu", new MenuDefinition("MainMenu", "主导航条")}
                    };
        }

        public void Initialize()
        {
            var context = new NavigationProviderContext(this);
        }
    }
}

        