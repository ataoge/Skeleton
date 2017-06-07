using System;

namespace Ataoge.Infrastructure
{
    public class NavigationConfigurationBuilder
    {
        private readonly SkeletonOptionsBuilder _optionsBuilder;

        public NavigationConfigurationBuilder(SkeletonOptionsBuilder optionsBuilder)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));

            _optionsBuilder = optionsBuilder;
        }

        public virtual NavigationConfigurationBuilder AddProvider(Type providerType)
            => WithOption(e => e.WithProvider(providerType));

        private NavigationConfigurationBuilder WithOption(Func<NavigationConfiguration, NavigationConfiguration> withFunc)
        {
            var coreOptionsExtension = _optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();

            ((ISkeletonOptionsBuilderInfrastructure)_optionsBuilder).AddOrUpdateExtension(
                coreOptionsExtension.WithNavigationConfiguration(withFunc(coreOptionsExtension.NavigationConfiguration)));

            return this;
        }
    }
}