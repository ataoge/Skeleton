using System;
using System.Linq;
using Ataoge.Infrastructure;
using JetBrains.Annotations;

namespace Ataoge
{
    public class SkeletonOptionsBuilder : ISkeletonOptionsBuilderInfrastructure
    {
        private SkeletonOptions _options;

        public SkeletonOptionsBuilder()
            : this(new SkeletonOptions())
        {
        }

        public SkeletonOptionsBuilder([NotNull] SkeletonOptions options)
        {
            Check.NotNull(options, nameof(options));

            _options = options;
        }

        public virtual SkeletonOptions Options => _options;

        public virtual bool IsConfigured => _options.Extensions.Any();

        public virtual SkeletonOptionsBuilder ReplaceService<TService, TImplementation>() where TImplementation : TService
            => WithOption(e => e.WithReplacedService(typeof(TService), typeof(TImplementation)));

        void ISkeletonOptionsBuilderInfrastructure.AddOrUpdateExtension<TExtension>(TExtension extension)
        {
            Check.NotNull(extension, nameof(extension));

            _options = _options.WithExtension(extension);
        }

        private SkeletonOptionsBuilder WithOption(Func<CoreOptionsExtension, CoreOptionsExtension> withFunc)
        {
            ((ISkeletonOptionsBuilderInfrastructure)this).AddOrUpdateExtension(
                withFunc(Options.FindExtension<CoreOptionsExtension>() ?? new CoreOptionsExtension()));

            return this;
        }

        public virtual SkeletonOptionsBuilder UseUserIdentifier([CanBeNull] Type userIdentifier)
            => WithOption(e => e.WithUserIndentifier(userIdentifier));

        public virtual SkeletonOptionsBuilder ConfigureMultiTenancy(
            [NotNull] Action<MultiTenancyConfigBuilder> multiTenancyConfigBuilderAction)
        {
            Check.NotNull(multiTenancyConfigBuilderAction, nameof(multiTenancyConfigBuilderAction));

            multiTenancyConfigBuilderAction(new MultiTenancyConfigBuilder(this));

            return this;
        }


        public virtual SkeletonOptionsBuilder ConfigureNavigation(
            [NotNull] Action<NavigationConfigurationBuilder> navigationConfigurationBuilderAction)
        {
            Check.NotNull(navigationConfigurationBuilderAction, nameof(navigationConfigurationBuilderAction));

            navigationConfigurationBuilderAction(new NavigationConfigurationBuilder(this));

            return this;
        }

    }
}