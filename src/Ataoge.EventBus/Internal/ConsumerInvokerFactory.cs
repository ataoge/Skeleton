using System;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.Internal
{
    internal class ConsumerInvokerFactory : IConsumerInvokerFactory
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IMessagePacker _messagePacker;
        private readonly IModelBinderFactory _modelBinderFactory;
        private readonly IServiceProvider _serviceProvider;

        public ConsumerInvokerFactory(
            ILoggerFactory loggerFactory,
            IMessagePacker messagePacker,
            IModelBinderFactory modelBinderFactory,
            IServiceProvider serviceProvider)
        {
            _loggerFactory = loggerFactory;
            _messagePacker = messagePacker;
            _modelBinderFactory = modelBinderFactory;
            _serviceProvider = serviceProvider;
        }

        public IConsumerInvoker CreateInvoker()
        {
            return new DefaultConsumerInvoker(_loggerFactory, _serviceProvider, _messagePacker, _modelBinderFactory);
        }
    }
}