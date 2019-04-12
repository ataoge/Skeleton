using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Abstractions;
using Ataoge.EventBus.Diagnostics;
using Ataoge.EventBus.Internal;
using Ataoge.EventBus.Models;
using Ataoge.EventBus.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus
{
    public abstract class PublisherBase : IPublisher
    {
        private readonly TransactionBase _transaction;
        private readonly IMessagePacker _msgPacker;
        private readonly IContentSerializer _serializer;

        protected bool NotUseTransaction;

        // ReSharper disable once InconsistentNaming
        protected static readonly DiagnosticListener s_diagnosticListener =
            new DiagnosticListener(EventBusDiagnosticListenerExtensions.DiagnosticListenerName);

        protected PublisherBase(IServiceProvider service)
        {
            ServiceProvider = service;
            _transaction = service.GetRequiredService<TransactionBase>();
            _msgPacker = service.GetRequiredService<IMessagePacker>();
            _serializer = service.GetRequiredService<IContentSerializer>();
        }

        protected IServiceProvider ServiceProvider { get; }


        public ITransaction Transaction => _transaction;

        public void Publish<T>(string name, T contentObj, string callbackName = null)
        {
             var message = new PublishedMessage
            {
                Id = SnowflakeId.Default().NextId(),
                Name = name,
                Content = Serialize(contentObj, callbackName),
                StatusName = StatusName.Scheduled
            };

            PublishAsyncInternal(message).GetAwaiter().GetResult();
        }

        public async Task PublishAsync<T>(string name, T contentObj, string callbackName = null, CancellationToken cancellationToken = default(CancellationToken))
        {
            var message = new PublishedMessage
            {
                Id = SnowflakeId.Default().NextId(),
                Name = name,
                Content = Serialize(contentObj, callbackName),
                StatusName = StatusName.Scheduled
            };

            await PublishAsyncInternal(message);
        }

        protected async Task PublishAsyncInternal(PublishedMessage message)
        {
            if (Transaction.DbTransaction == null)
            {
                NotUseTransaction = true;
                Transaction.DbTransaction = new NoopTransaction();
            }

            Guid operationId = default(Guid);
            try
            {
                operationId = s_diagnosticListener.WritePublishMessageStoreBefore(message);

                await ExecuteAsync(message,Transaction);
                
                _transaction.AddToSent(message);

                s_diagnosticListener.WritePublishMessageStoreAfter(operationId, message);

                if (NotUseTransaction || Transaction.AutoCommit)
                {
                    _transaction.Commit();
                }
            }
            catch (Exception e)
            {
                s_diagnosticListener.WritePublishMessageStoreError(operationId, message, e);
                throw;
            }
            finally
            {
                if (NotUseTransaction || Transaction.AutoCommit)
                {
                    Transaction.Dispose();
                }
            }
        }


        protected abstract Task ExecuteAsync(PublishedMessage message,
            ITransaction transaction,
            CancellationToken cancel = default(CancellationToken));

        protected virtual string Serialize<T>(T obj, string callbackName = null)
        {
            string content;
            if (obj != null)
            {
                content = CommonHelper.IsComplexType(obj.GetType())
                    ? _serializer.Serialize(obj)
                    : obj.ToString();
            }
            else
            {
                content = string.Empty;
            }
            var message = new MessageDto(content)
            {
                CallbackName = callbackName
            };
            return _msgPacker.Pack(message);
        }
    }
}