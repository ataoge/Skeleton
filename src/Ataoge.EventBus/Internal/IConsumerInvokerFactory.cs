namespace Ataoge.EventBus.Internal
{
    internal interface IConsumerInvokerFactory
    {
        IConsumerInvoker CreateInvoker();
    }
}