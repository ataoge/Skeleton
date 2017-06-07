using System;
using System.Collections.Generic;
using System.Linq;
using Ataoge.Infrastructure;
using JetBrains.Annotations;

namespace Ataoge
{
    public class SkeletonOptions : ISkeletonOptions
    {
        public SkeletonOptions()
            : this(new Dictionary<Type, ISkeletonOptionsExtension>())
        {

        }

        public SkeletonOptions([NotNull] IReadOnlyDictionary<Type, ISkeletonOptionsExtension> extensions)
        {
            _extensions = extensions;
        }

        public virtual IEnumerable<ISkeletonOptionsExtension> Extensions => _extensions.Values;

        public virtual TExtension FindExtension<TExtension>()
            where TExtension : class, ISkeletonOptionsExtension
        {
            ISkeletonOptionsExtension extension;
            return _extensions.TryGetValue(typeof(TExtension), out extension) ? (TExtension)extension : null;
        }

        public virtual TExtension GetExtension<TExtension>()
            where TExtension : class, ISkeletonOptionsExtension
        {
            var extension = FindExtension<TExtension>();
            if (extension == null)
            {
                throw new InvalidOperationException(typeof(TExtension).FullName);
            }
            return extension;
        }

        private readonly IReadOnlyDictionary<Type, ISkeletonOptionsExtension> _extensions;

        public virtual SkeletonOptions WithExtension<TExtension>([NotNull] TExtension extension)
            where TExtension : class, ISkeletonOptionsExtension
        {
            Check.NotNull(extension, nameof(extension));

            var extensions = Extensions.ToDictionary(p => p.GetType(), p => p);
            extensions[typeof(TExtension)] = extension;

            return new SkeletonOptions(extensions);
        }

    }
}