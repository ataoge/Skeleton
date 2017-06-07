using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.Infrastructure
{
     public interface ISkeletonOptionsExtension
     {
         //bool ApplyServices([NotNull] IServiceCollection services);
         
         //long GetServiceProviderHashCode();

         void Validate([NotNull] ISkeletonOptions options);
     }
}