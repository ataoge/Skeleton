using System;

namespace Ataoge.EventBus.Diagnostics
{
    public interface IErrorEventData
    {
        Exception Exception { get; }
    }
}