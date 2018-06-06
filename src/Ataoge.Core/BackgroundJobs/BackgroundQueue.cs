using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ataoge.BackgroundJobs
{   
    public class BackgroundQueue : ITask
    {
        //private readonly Action<Exception> _onException;

        internal readonly ConcurrentQueue<IRunnable> TaskQueue = new ConcurrentQueue<IRunnable>();

        internal readonly int MaxConcurrentCount;

        internal readonly int MilllisecondsToWaitBeforePickingUpTask;

        internal int ConcurrentCount;


        private CancellationTokenSource cancellationTokenSource;

        private ILogger logger;

        private IServiceScopeFactory ServiceScopeFactory {get;}
        public int Progresss { get; set; } = 0;
        public string CurrentTaskId { get; set; }

        public BackgroundQueue(ILoggerFactory loggerFactory, IServiceScopeFactory serviceScopeFactory, BackgroundQueueConfig config)
        {
            this.logger = loggerFactory.CreateLogger<BackgroundQueue>();
            this.ServiceScopeFactory = serviceScopeFactory;

            this.MaxConcurrentCount = config.MaxConcurrentCount;
            this.MilllisecondsToWaitBeforePickingUpTask = config.MilllisecondsToWaitBeforePickingUpTask;
        }

        protected void OnException(Exception exception)
        {

        }

        public void Enqueue(IRunnable runnable)
        {
            TaskQueue.Enqueue(runnable);
        }

        public void Stop()
        {
            if (!string.IsNullOrEmpty(this.CurrentTaskId))
            {
                cancellationTokenSource?.Cancel();
            }
        }

        internal async Task Dequeue(CancellationToken cancellationToken)
        {
            cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            if (TaskQueue.TryDequeue(out var runnable))
            {
                Interlocked.Increment(ref ConcurrentCount);
                try
                {
                    using (var scope = ServiceScopeFactory.CreateScope())
                    {
                        this.Progresss = 0;
                        this.CurrentTaskId = runnable.TaskId;
                        await runnable.RunAsync(this, scope.ServiceProvider, cancellationToken);
                        this.Progresss = 0;
                        this.CurrentTaskId = string.Empty;
                    }
                }
                catch(Exception e)
                {
                    OnException(e);
                }
                finally
                {
                    Interlocked.Decrement(ref ConcurrentCount);
                    
                }
            }

            await Task.CompletedTask;
        }


        
    }
}