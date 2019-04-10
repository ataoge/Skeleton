using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Ataoge.EventBus.Internal;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Diagnostics
{
    public static class EventBusDiagnosticListenerExtensions
    {
        public const string DiagnosticListenerName = "EventBusDiagnosticListener";

        private const string Prefix = "Ataoge.EventBus.";

        public const string BeforePublishMessageStore = Prefix + nameof(WritePublishMessageStoreBefore);
        public const string AfterPublishMessageStore = Prefix + nameof(WritePublishMessageStoreAfter);
        public const string ErrorPublishMessageStore = Prefix + nameof(WritePublishMessageStoreError);

         public const string BeforePublish = Prefix + nameof(WritePublishBefore);
        public const string AfterPublish = Prefix + nameof(WritePublishAfter);
        public const string ErrorPublish = Prefix + nameof(WritePublishError);

        public const string BeforeConsume = Prefix + nameof(WriteConsumeBefore);
        public const string AfterConsume = Prefix + nameof(WriteConsumeAfter);
        public const string ErrorConsume = Prefix + nameof(WriteConsumeError);

        public const string BeforeSubscriberInvoke = Prefix + nameof(WriteSubscriberInvokeBefore);
        public const string AfterSubscriberInvoke = Prefix + nameof(WriteSubscriberInvokeAfter);
        public const string ErrorSubscriberInvoke = Prefix + nameof(WriteSubscriberInvokeError);

        //============================================================================
        //====================  Before publish store message      ====================
        //============================================================================
        public static Guid WritePublishMessageStoreBefore(this DiagnosticListener @this,
            PublishedMessage message,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(BeforePublishMessageStore))
            {
                var operationId = Guid.NewGuid();

                @this.Write(BeforePublishMessageStore, new
                {
                    OperationId = operationId,
                    Operation = operation,
                    MessageName = message.Name,
                    MessageContent = message.Content
                });

                return operationId;
            }

            return Guid.Empty;
        }

        public static void WritePublishMessageStoreAfter(this DiagnosticListener @this,
            Guid operationId,
            PublishedMessage message,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(AfterPublishMessageStore))
            {
                @this.Write(AfterPublishMessageStore, new
                {
                    OperationId = operationId,
                    Operation = operation,
                    MessageId = message.Id,
                    MessageName = message.Name,
                    MessageContent = message.Content,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        public static void WritePublishMessageStoreError(this DiagnosticListener @this,
            Guid operationId,
            PublishedMessage message,
            Exception ex,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(ErrorPublishMessageStore))
            {
                @this.Write(ErrorPublishMessageStore, new
                {
                    OperationId = operationId,
                    Operation = operation,
                    MessageName = message.Name,
                    MessageContent = message.Content,
                    Exception = ex,
                    Timestamp = Stopwatch.GetTimestamp()
                });
            }
        }

        //============================================================================
        //====================                  Publish           ====================
        //============================================================================
        public static void WritePublishBefore(this DiagnosticListener @this, BrokerPublishEventData eventData)
        {
            if (@this.IsEnabled(BeforePublish))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(BeforePublish, eventData);
            }
        }

        public static void WritePublishAfter(this DiagnosticListener @this, BrokerPublishEndEventData eventData)
        {
            if (@this.IsEnabled(AfterPublish))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(AfterPublish, eventData);
            }
        }

        public static void WritePublishError(this DiagnosticListener @this, BrokerPublishErrorEventData eventData)
        {
            if (@this.IsEnabled(ErrorPublish))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(ErrorPublish, eventData);
            }
        }


        //============================================================================
        //====================                  Consume           ====================
        //============================================================================
        public static Guid WriteConsumeBefore(this DiagnosticListener @this, BrokerConsumeEventData eventData)
        {
            if (@this.IsEnabled(BeforeConsume))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(BeforeConsume, eventData);
            }

            return Guid.Empty;
        }

        public static void WriteConsumeAfter(this DiagnosticListener @this, BrokerConsumeEndEventData eventData)
        {
            if (@this.IsEnabled(AfterConsume))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(AfterConsume, eventData);
            }
        }

        public static void WriteConsumeError(this DiagnosticListener @this, BrokerConsumeErrorEventData eventData)
        {
            if (@this.IsEnabled(ErrorConsume))
            {
                eventData.Headers = new TracingHeaders();
                @this.Write(ErrorConsume, eventData);
            }
        }


        //============================================================================
        //====================           SubscriberInvoke         ====================
        //============================================================================
        public static Guid WriteSubscriberInvokeBefore(this DiagnosticListener @this,
            ConsumerContext context,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(BeforeSubscriberInvoke))
            {
                var operationId = Guid.NewGuid();

                var methodName = context.ConsumerDescriptor.MethodInfo.Name;
                var subscribeName = context.ConsumerDescriptor.Attribute.Name;
                var subscribeGroup = context.ConsumerDescriptor.Attribute.Group;
                var parameterValues = context.DeliverMessage.Content;

                @this.Write(BeforeSubscriberInvoke, new SubscriberInvokeEventData(operationId, operation, methodName,
                    subscribeName,
                    subscribeGroup, parameterValues, DateTimeOffset.UtcNow));

                return operationId;
            }

            return Guid.Empty;
        }

        public static void WriteSubscriberInvokeAfter(this DiagnosticListener @this,
            Guid operationId,
            ConsumerContext context,
            DateTimeOffset startTime,
            TimeSpan duration,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(AfterSubscriberInvoke))
            {
                var methodName = context.ConsumerDescriptor.MethodInfo.Name;
                var subscribeName = context.ConsumerDescriptor.Attribute.Name;
                var subscribeGroup = context.ConsumerDescriptor.Attribute.Group;
                var parameterValues = context.DeliverMessage.Content;

                @this.Write(AfterSubscriberInvoke, new SubscriberInvokeEndEventData(operationId, operation, methodName,
                    subscribeName,
                    subscribeGroup, parameterValues, startTime, duration));
            }
        }

        public static void WriteSubscriberInvokeError(this DiagnosticListener @this,
            Guid operationId,
            ConsumerContext context,
            Exception ex,
            DateTimeOffset startTime,
            TimeSpan duration,
            [CallerMemberName] string operation = "")
        {
            if (@this.IsEnabled(ErrorSubscriberInvoke))
            {
                var methodName = context.ConsumerDescriptor.MethodInfo.Name;
                var subscribeName = context.ConsumerDescriptor.Attribute.Name;
                var subscribeGroup = context.ConsumerDescriptor.Attribute.Group;
                var parameterValues = context.DeliverMessage.Content;

                @this.Write(ErrorSubscriberInvoke, new SubscriberInvokeErrorEventData(operationId, operation, methodName,
                    subscribeName,
                    subscribeGroup, parameterValues, ex, startTime, duration));
            }
        }
    }
}
