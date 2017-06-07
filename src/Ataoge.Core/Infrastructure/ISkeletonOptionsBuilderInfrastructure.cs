using JetBrains.Annotations;

namespace Ataoge.Infrastructure
{
    public interface ISkeletonOptionsBuilderInfrastructure
    {
        void AddOrUpdateExtension<TExtension>([NotNull] TExtension extension)
            where TExtension : class, ISkeletonOptionsExtension;
    }
}