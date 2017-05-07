using System;
using System.IO;
using Ataoge.Configuration;
using Ataoge.Modules;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Ataoge.Core.Tests
{
    public class UnitTest1
    {
        private class ExampleOption
        { 
            public int[] Array {get;set;}
        }
        public UnitTest1()
        {
             Console.WriteLine($"{Directory.GetCurrentDirectory()}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration {get; set;}
        
        [Fact]
        public void Test1()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();

            services.Configure<ExampleOption>(Configuration.GetSection("ExampleOption"));

            IServiceProvider sp = services.BuildServiceProvider();

            IOptions<ExampleOption> optionsAccesstor = sp.GetService<IOptions<ExampleOption>>();
            Console.WriteLine($"{optionsAccesstor.Value.Array[0]}, {optionsAccesstor.Value.Array[1]}");

            var result = false;
             Assert.False(result, $"1 should not be prime");
        }

          [Fact]
        public void TestStartupConfiguration()
        {
            IServiceCollection services = new ServiceCollection();
            IModuleManager modulManager = new ModuleManager();
            modulManager.Initialize(typeof(KernelModule));
            modulManager.ConfigModules(services);
            services.AddSingleton<IModuleManager>(modulManager);
            IServiceProvider sp = services.BuildServiceProvider();
            IStartupConfiguration startupConfig = sp.GetService<IStartupConfiguration>();
            Assert.False(startupConfig.MultiTenancy.IsEnabled,$"1 should not be prime");
        }
    }
}
