using System;
using System.Collections.Generic;
using Ataoge.Dependency;
using Ataoge.Infrastructure;

namespace Ataoge.Application.Navigation
{
    internal class NavigationManager : INavigationManager, ISingletonDependency
    {
        public IDictionary<string, MenuDefinition> Menus { get; private set; }

        public MenuDefinition MainMenu
        {
            get { return Menus["MainMenu"]; }
        }

        //private readonly IServiceProvider _sp;
        private readonly INavigationConfiguration _configuration;

        public NavigationManager(INavigationConfiguration configuration)
        {
            //_sp = servierProvider;
            //_iocResolver = iocResolver;
            _configuration = configuration;

            Menus = new Dictionary<string, MenuDefinition>
                    {
                        {"MainMenu", new MenuDefinition("MainMenu", "主导航条")}
                    };
        }

        public void Initialize(IServiceProvider serviceProvider)
        {
            var context = new NavigationProviderContext(this);
            foreach (var providerType in _configuration.Providers)
            {
                var provider = serviceProvider.GetService(providerType) as NavigationProvider;
                provider.SetNavigation(context);
            }
        }
    }
}

        