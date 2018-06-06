using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Ataoge.BackgroundJobs
{
    public abstract class HostedService : IHostedService
    {
        private Task _executingTask;
        private CancellationTokenSource _cancellationTokenSource;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //创建链接的令牌，以便在这个令牌的cancellation外部触发cancellation
            _cancellationTokenSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            //存储正在执行的任务
            _executingTask = ExecuteAsync(_cancellationTokenSource.Token);

            //如果任务完成则返回任务本身，否则它正在运行
            return _executingTask.IsCompleted ? _executingTask : Task.CompletedTask;

        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_executingTask == null)
                return;
            
            //对正在执行的方法发送cancdellation信号
            _cancellationTokenSource.Cancel();

            //等待直到任务完成，或者停止令牌触发
            await Task.WhenAny(_executingTask, Task.Delay(-1, cancellationToken));

            //抛出如果cancellation 触发
            cancellationToken.ThrowIfCancellationRequested();
        }

        protected abstract Task ExecuteAsync(CancellationToken cancellationToken);
    }
}