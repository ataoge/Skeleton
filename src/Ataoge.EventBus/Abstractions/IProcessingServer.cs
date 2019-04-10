using System;

namespace Ataoge.EventBus
{
    /// <summary>
    /// 处理服务器。
    /// A process thread abstract of message process.
    /// </summary>
    public interface IProcessingServer : IDisposable
    {
        void Pulse();

        void Start();
    }
}