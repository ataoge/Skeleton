using System;
using System.Threading;
using System.Threading.Tasks;

namespace Ataoge.BackgroundJobs
{
    public interface IRunnable
    {
        string  TaskId {get;}
        
        Task RunAsync(ITask task, IServiceProvider scopeServiceProvider, CancellationToken CancellationToken);
    }
    
}