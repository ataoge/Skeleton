using System;
using Ataoge.BackgroundJobs;
using Ataoge.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge
{
    public static class BackgroundQueueConfigBuilderExtensions
    {
        public static SkeletonOptionsBuilder ConfigBackgroundQueue(this SkeletonOptionsBuilder builder,  Action<BackgroundQueueConfig> setupAction)
        {
            BackgroundQueueConfig config = new BackgroundQueueConfig();
            if (setupAction != null)
                setupAction(config);
            
            builder.Services.AddSingleton<BackgroundQueueConfig>(config);

            return builder;
        }


    }

    
}