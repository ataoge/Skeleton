using System;
using Ataoge.ObjectMapping;
using Ataoge.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System.Reflection;

namespace Ataoge.AutoMapper.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAtaogeSketleton<AtaogeAutoMapperModule>(opt => {
                 opt.UseAutoMapper(t => t.AddProfileAssembly(typeof(TestMapperProfile).GetTypeInfo().Assembly));
            });
            services.AddSingleton<TestSingleton>();
            services.AddScoped<TestValueResolver>();
           
            IServiceProvider sp = services.BuildServiceProvider();
            IObjectMapper  _map = sp.GetRequiredService<IObjectMapper>(); 
            var result = _map.Map<TestDto>(new TestEntity() { Id = 10 });
        }
    }

    

    public class TestEntity
    {
        public int Id {get; set;}

        public string Uid {get; set;}
    }

    public class TestDto
    {
        public int Id {get;set;}

        public string Url {get; set;}
    }

  

    
}
