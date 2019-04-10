using System;
using System.Text;
using System.Threading.Tasks;
using Ataoge.EventBus.Processor.States;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Framing;

namespace Ataoge.EventBus.RabbitMQ
{
    internal sealed class RabbitMQPublishMessageSender : PublishMessageSenderBase
    {
        private readonly IConnectionChannelPool _connectionChannelPool;
        private readonly ILogger _logger;
        private readonly string _exchange;
        
        public RabbitMQPublishMessageSender(ILogger<RabbitMQPublishMessageSender> logger, ConfigOptions options,
            IStorageConnection connection, IConnectionChannelPool connectionChannelPool, IStateChanger stateChanger)
            : base(logger, options, connection, stateChanger)
        {
            _logger = logger;
            _connectionChannelPool = connectionChannelPool;
            _exchange = _connectionChannelPool.Exchange;
            ServersAddress = _connectionChannelPool.HostAddress;
        }

        public override Task<OperateResult> PublishAsync(string keyName, string content)
        {
            var channel = _connectionChannelPool.Rent();
            try
            {
                var body = Encoding.UTF8.GetBytes(content);
                var props = new BasicProperties()
                {
                    DeliveryMode = 2
                };

                channel.ExchangeDeclare(_exchange, RabbitMQOptions.ExchangeType, true);
                channel.BasicPublish(_exchange, keyName, props, body);

                _logger.LogDebug($"RabbitMQ topic message [{keyName}] has been published.");

                return Task.FromResult(OperateResult.Success);
            }
            catch (Exception ex)
            {
                var wapperEx = new PublisherSentFailedException(ex.Message, ex);
                var errors = new OperateError
                {
                    Code = ex.HResult.ToString(),
                    Description = ex.Message
                };

                return Task.FromResult(OperateResult.Failed(wapperEx, errors));
            }
            finally
            {
                var returned = _connectionChannelPool.Return(channel);
                if (!returned)
                {
                    channel.Dispose();
                }
            }
        
        }
    }
}