using System;
using Ataoge.Application.Navigation;
using Ataoge.Collections;
using JetBrains.Annotations;

namespace Ataoge.Infrastructure
{
    public class NavigationConfiguration  : INavigationConfiguration
    {

        private  ITypeList<NavigationProvider> _providers;
        public ITypeList<NavigationProvider> Providers => _providers;

        public NavigationConfiguration()
        {
            _providers = new TypeList<NavigationProvider>();
        }


        protected NavigationConfiguration([NotNull] NavigationConfiguration copyFrom)
        {
            _providers = new TypeList<NavigationProvider>();
            if (copyFrom._providers.Count > 0)
            {
                foreach(var type in copyFrom._providers)
                {
                    _providers.Add(type);
                }
            }
        }

        protected virtual NavigationConfiguration Clone() => new NavigationConfiguration(this);

        public virtual NavigationConfiguration WithProvider(Type navigationProviderType)
        {
            var clone = Clone();

            clone._providers.Add(navigationProviderType);

            return clone;
        }
    }

}