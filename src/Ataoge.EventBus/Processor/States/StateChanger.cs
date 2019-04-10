using System;
using Ataoge.EventBus.Models;

namespace Ataoge.EventBus.Processor.States
{
    public class StateChanger : IStateChanger
    {
        public void ChangeState(PublishedMessage message, IState state, IStorageTransaction transaction)
        {
            var now = DateTime.Now;
            if (state.ExpiresAfter != null)
            {
                message.ExpiresAt = now.Add(state.ExpiresAfter.Value);
            }
            else
            {
                message.ExpiresAt = null;
            }

            message.StatusName = state.Name;
            state.Apply(message, transaction);
            transaction.UpdateMessage(message);
        }

        public void ChangeState(ReceivedMessage message, IState state, IStorageTransaction transaction)
        {
            var now = DateTime.Now;
            if (state.ExpiresAfter != null)
            {
                message.ExpiresAt = now.Add(state.ExpiresAfter.Value);
            }
            else
            {
                message.ExpiresAt = null;
            }

            message.StatusName = state.Name;
            state.Apply(message, transaction);
            transaction.UpdateMessage(message);
        }
    }
}