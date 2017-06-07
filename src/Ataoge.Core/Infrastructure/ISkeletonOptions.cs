using System.Collections.Generic;

namespace Ataoge.Infrastructure
{
     public interface ISkeletonOptions
     {
         IEnumerable<ISkeletonOptionsExtension> Extensions { get; }

         TExtension FindExtension<TExtension>() where TExtension : class, ISkeletonOptionsExtension;
     }
}