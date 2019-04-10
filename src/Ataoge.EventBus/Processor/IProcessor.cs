using System.Threading.Tasks;

namespace Ataoge.EventBus.Processor
{
    /// <summary>
    /// 在处理管道中，不同的处理机执行不同的处理任务
    /// </summary>
    public interface IProcessor
    {
        Task ProcessAsync(ProcessingContext context);
    }
}