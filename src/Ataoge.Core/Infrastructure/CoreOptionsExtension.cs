using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ataoge.Infrastructure
{
    public class CoreOptionsExtension : ISkeletonOptionsExtension
    {
        public CoreOptionsExtension()
        {
        }

        public CoreOptionsExtension([NotNull] CoreOptionsExtension copyFrom)
        {
            _userIndentifier = copyFrom.UserIndentifier;
            _multiTenancyConfig = copyFrom.MultiTenancyConfig;


            if (copyFrom._replacedServices != null)
            {
                _replacedServices = new Dictionary<Type, Type>(copyFrom._replacedServices);
            }
        }

        private IDictionary<Type, Type> _replacedServices;

        private MultiTenancyConfig _multiTenancyConfig = new MultiTenancyConfig();
        public virtual MultiTenancyConfig MultiTenancyConfig => _multiTenancyConfig;


        private NavigationConfiguration _navigationConfiguration = new NavigationConfiguration();
        public virtual NavigationConfiguration NavigationConfiguration => _navigationConfiguration;
        
        private Type _userIndentifier;
        public Type UserIndentifier => _userIndentifier;

        protected virtual CoreOptionsExtension Clone() => new CoreOptionsExtension(this);


        public virtual CoreOptionsExtension WithUserIndentifier([CanBeNull] Type userIndentifier)
        {
            var clone = Clone();

            clone._userIndentifier = userIndentifier;


            return clone;
        }

        public virtual CoreOptionsExtension WithMultiTenancyConfig([CanBeNull] MultiTenancyConfig multiTenancyConfig)
        {
            var clone = Clone();

            clone._multiTenancyConfig = multiTenancyConfig;

            return clone;
        }

        public virtual CoreOptionsExtension WithNavigationConfiguration([CanBeNull] NavigationConfiguration navigationConfiguration)
        {
            var clone = Clone();

            clone._navigationConfiguration = navigationConfiguration;

            return clone;
        }

        public virtual CoreOptionsExtension WithReplacedService([NotNull] Type serviceType, [NotNull] Type implementationType)
        {
            var clone = Clone();

            if (clone._replacedServices == null)
            {
                clone._replacedServices = new Dictionary<Type, Type>();
            }

            clone._replacedServices[serviceType] = implementationType;

            return clone;
        }

        public virtual long GetServiceProviderHashCode()
        {
            return 0;
        }

        public virtual void Validate(ISkeletonOptions options)
        {
        }
    }
}