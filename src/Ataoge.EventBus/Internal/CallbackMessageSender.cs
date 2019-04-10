using System;
using System.Threading.Tasks;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Models;
using Ataoge.EventBus.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ataoge.EventBus.Internal
{
    internal class CallbackMessageSender : ICallbackMessageSender
    {
        private readonly IContentSerializer _contentSerializer;
        private readonly ILogger<CallbackMessageSender> _logger;
        private readonly IMessagePacker _messagePacker;
        private readonly IServiceProvider _serviceProvider;

        public CallbackMessageSender(
            ILogger<CallbackMessageSender> logger,
            IServiceProvider serviceProvider,
            IContentSerializer contentSerializer,
            IMessagePacker messagePacker)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _contentSerializer = contentSerializer;
            _messagePacker = messagePacker;
        }

        public async Task SendAsync(string messageId, string topicName, object bodyObj)
        {
            string body;
            if (bodyObj != null && CommonHelper.IsComplexType(bodyObj.GetType()))
            {
                body = _contentSerializer.Serialize(bodyObj);
            }
            else
            {
                body = bodyObj?.ToString();
            }

            _logger.LogDebug($"Callback message will publishing, name:{topicName},content:{body}");

            var callbackMessage = new MessageDto
            {
                Id = messageId,
                Content = body
            };

            var content = _messagePacker.Pack(callbackMessage);

            var publishedMessage = new PublishedMessage
            {
                Id = SnowflakeId.Default().NextId(),
                Name = topicName,
                Content = content,
                StatusName = StatusName.Scheduled
            };

            using (var scope = _serviceProvider.CreateScope())
            {
                var provider = scope.ServiceProvider;
                var callbackPublisher = provider.GetService<ICallbackPublisher>();
                await callbackPublisher.PublishCallbackAsync(publishedMessage);
            }
        }
    }
}