using System;
using JetBrains.Annotations;

namespace Ataoge.Infrastructure
{
    public abstract class SkeletonOptionsExtensionBuilder<TBuilder, TExtension> 
         where TBuilder : SkeletonOptionsExtensionBuilder<TBuilder, TExtension>
         where TExtension : class, ISkeletonOptionsExtension, new()
    {
        protected SkeletonOptionsExtensionBuilder([NotNull] SkeletonOptionsBuilder optionsBuilder)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));

            OptionsBuilder = optionsBuilder;
        }

        /// <summary>
        ///     Gets the core options builder.
        /// </summary>
        protected virtual SkeletonOptionsBuilder OptionsBuilder { get; }

        protected virtual TBuilder WithOption([NotNull] Func<TExtension, TExtension> setAction)
        {
            ((ISkeletonOptionsBuilderInfrastructure)OptionsBuilder).AddOrUpdateExtension(
                setAction(OptionsBuilder.Options.FindExtension<TExtension>() ?? new TExtension()));

            return (TBuilder)this;
        }
    }
}