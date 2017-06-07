using System;
using System.Reflection;
using Ataoge.Configuration;

namespace Ataoge.Infrastructure
{
    public static class AutoMapperSkeletonOptionsBuilderExtension
    {
        public static SkeletonOptionsBuilder UseAutoMapper(this SkeletonOptionsBuilder optionsBuilder, Action<AutoMapperConfigurationBuilder> optionsAction = null)
        {
            var extension = (AutoMapperConfiguration)GetOrCreateExtension(optionsBuilder);
            ((ISkeletonOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            optionsAction?.Invoke(new AutoMapperConfigurationBuilder(optionsBuilder));

            return optionsBuilder;
        }


        private static AutoMapperConfiguration GetOrCreateExtension(SkeletonOptionsBuilder options)
            => options.Options.FindExtension<AutoMapperConfiguration>()
               ?? new AutoMapperConfiguration();
    }
}