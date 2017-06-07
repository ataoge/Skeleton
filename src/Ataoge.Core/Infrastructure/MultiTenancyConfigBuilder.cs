using System;

namespace Ataoge.Infrastructure
{
    public class MultiTenancyConfigBuilder
    {
        private readonly SkeletonOptionsBuilder _optionsBuilder;

        public MultiTenancyConfigBuilder(SkeletonOptionsBuilder optionsBuilder)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));

            _optionsBuilder = optionsBuilder;
        }

        public virtual MultiTenancyConfigBuilder Enabled(bool enabled)
            => WithOption(e => e.WithIsEnabled(enabled));

        private MultiTenancyConfigBuilder WithOption(Func<MultiTenancyConfig, MultiTenancyConfig> withFunc)
        {
            var coreOptionsExtension = _optionsBuilder.Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension();

            ((ISkeletonOptionsBuilderInfrastructure)_optionsBuilder).AddOrUpdateExtension(
                coreOptionsExtension.WithMultiTenancyConfig(withFunc(coreOptionsExtension.MultiTenancyConfig)));

            return this;
        }
    }
}