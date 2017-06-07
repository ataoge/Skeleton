using System;
using Ataoge.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.AspNetCore
{
    public static class SkeletonApplicationBuilderExtensions
    {
        public static void UseAtaogeSkeleton(this IApplicationBuilder app)
        {
            InitializeAtaogeSkeleton(app);
        }

        private static void InitializeAtaogeSkeleton(IApplicationBuilder app)
        {
            var moduleManager = app.ApplicationServices.GetRequiredService<IModuleManager>();
            moduleManager.StartModules(app.ApplicationServices);
        }
    }
}