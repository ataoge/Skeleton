using System;
using Ataoge.Dependency;

namespace Ataoge.ObjectMapping
{
    public sealed class NullObjectMapper : IObjectMapper, ISingletonDependency
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullObjectMapper Instance { get { return SingletonInstance; } }
        private static readonly NullObjectMapper SingletonInstance = new NullObjectMapper();

        public TDestination Map<TDestination>(object source, Action<IMapperContext> operationAction = null)
        {
            throw new SafException("Ataoge.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMapperContext> operationAction = null)
        {
            throw new SafException("Ataoge.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TDestination>(object source)
        {
            throw new SafException("Ataoge.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            throw new SafException("Ataoge.ObjectMapping.IObjectMapper should be implemented in order to map objects.");
        }
    }
}