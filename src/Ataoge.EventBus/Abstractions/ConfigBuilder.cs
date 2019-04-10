using System;
using Ataoge.EventBus.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus
{

    /// <summary>
    /// Used to verify cap service was called on a ServiceCollection
    /// </summary>
    public class CapMarkerService
    {
    }

    /// <summary>
    /// Used to verify cap storage extension was added on a ServiceCollection
    /// </summary>
    public class CapStorageMarkerService
    {
    }

    /// <summary>
    /// Used to verify cap message queue extension was added on a ServiceCollection
    /// </summary>
    public class CapMessageQueueMakerService
    {
    }

    /// <summary>
    /// 允许更精细地配置CAP服务
    /// Allows fine grained configuration of CAP services.
    /// </summary>
    public sealed class ConfigBuilder
    {
        public ConfigBuilder(IServiceCollection services)
        {
            Services = services;
        }

        /// <summary>
        /// Gets the <see cref="IServiceCollection" /> where MVC services are configured.
        /// </summary>
        public IServiceCollection Services { get; }

        /// <summary>
        /// Add an <see cref="ICapPublisher" />.
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        public ConfigBuilder AddProducerService<T>()
            where T : class, IPublisher
        {
            return AddScoped(typeof(IPublisher), typeof(T));
        }

        /// <summary>
        /// Add a custom content serializer
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        public ConfigBuilder AddContentSerializer<T>()
            where T : class, IContentSerializer
        {
            return AddSingleton(typeof(IContentSerializer), typeof(T));
        }

        /// <summary>
        /// Add a custom message wapper
        /// </summary>
        /// <typeparam name="T">The type of the service.</typeparam>
        public ConfigBuilder AddMessagePacker<T>()
            where T : class, IMessagePacker
        {
            return AddSingleton(typeof(IMessagePacker), typeof(T));
        }

        /// <summary>
        /// Adds a scoped service of the type specified in serviceType with an implementation
        /// </summary>
        private ConfigBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        /// <summary>
        /// Adds a singleton service of the type specified in serviceType with an implementation
        /// </summary>
        private ConfigBuilder AddSingleton(Type serviceType, Type concreteType)
        {
            Services.AddSingleton(serviceType, concreteType);
            return this;
        }
    }
}