using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Processor.States
{
    public interface IStateChanger
    {
        void ChangeState(PublishedMessage message, IState state, IStorageTransaction transaction);

        void ChangeState(ReceivedMessage message, IState state, IStorageTransaction transaction);
    }
}