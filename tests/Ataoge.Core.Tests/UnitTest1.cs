using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Ataoge.Configuration;
using Ataoge.Data.Entities;
using Ataoge.Modules;
using Ataoge.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Xunit;

namespace Ataoge.Core.Tests
{
    public class UnitTest1
    {
        private class ExampleOption
        {
            public int[] Array { get; set; }
        }
        public UnitTest1()
        {
            Console.WriteLine($"{Directory.GetCurrentDirectory()}");

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

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
            IModuleManager modulManager = new ModuleManager(new SkeletonOptions());
            //modulManager.Initialize(typeof(AutoMapper.AtaogeAutoMapperModule));
            modulManager.ConfigModules(services);
            services.AddSingleton<IModuleManager>(modulManager);
            IServiceProvider sp = services.BuildServiceProvider();
            IStartupConfiguration startupConfig = sp.GetService<IStartupConfiguration>();
            Assert.False(startupConfig.MultiTenancy.IsEnabled, $"1 should not be prime");
        }

        [Fact]
        public void TestStringUtils()
        {
            var bb = Type.GetTypeCode(typeof(double)).ToString();
                     bb=    Type.GetTypeCode(typeof(DateTime)).ToString();
            bb=    Type.GetTypeCode(typeof(float)).ToString();
            var a = StringUtils.GenerateSequentialGuidString();
            var g = Guid.Parse(a);
        }

        [Fact]
        public void TestStringUtils2()
        {

            bool result = StringUtils.IsMobilePhone("13312341234");
        }


        [Fact]
        public void TestIdWorker()
        {
            HashSet<long> set = new HashSet<long>();
            IdWorker idWorker1 = new IdWorker(0, 0);
            IdWorker idWorker2 = new IdWorker(1, 0);
            //762884413578018816
            //762884520121729024
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 100; i++)
            {
                long id = idWorker1.NextId();
                set.Add(id);
                //if (!set.Add(id))
                //{
                //Console.WriteLine("duplicate:" + id);
                //}
            }
            sw.Stop();
            foreach (var item in set)
            {
                Console.WriteLine("结果:" + item);
            }
            Console.WriteLine("时间:" + sw.ElapsedTicks);
            return;
        }

        [Fact]
        public void TestDateTime()
        {
            
            var result = DateTime.Today.LastWeekRange();
        }

        [Fact]
        public void TestJsonData()
        {
            
            CommonDataEntity<int> aa = new CommonEntity();
            aa.Id = 1;
            aa["aa"] = "bbb";
            string json = JsonConvert.SerializeObject(aa);
            var bb = JsonConvert.DeserializeObject<CommonEntity>(json);
        }

         [Fact]
        public void TestPinyin()
        {

            var pinyin = PinyinHelper.GetPinyin("我是中国人", false);
        }

    }


    public class CommonEntity : CommonDataEntity<int>
    {
        [JsonExtensionData]
        private IDictionary<string, object> overrideDcits = new Dictionary<string, object>();

        public override object this[string key]
        {
            get 
            {
                if (overrideDcits.ContainsKey(key))
                    return overrideDcits[key];
                return null;
            }
            set
            {
                overrideDcits[key] = value;
            }
        }
    }
}
