using System;
using System.Reflection;
using Ataoge.DynamicProxy;
using Xunit;

namespace Ataoge.Core.Tests
{
    public class DynamicProxyTest
    {
        [Fact]
        public void Test1()
        {
            var poxy1 = (ITestInterface)ProxyGenerator.Create(typeof(ITestInterface), new SampleProxy("coreproxy1"));
            poxy1.Write("here was invoked"); //---> "here was invoked by coreproxy1"

            var poxy2 = (ITestInterface)ProxyGenerator.Create(typeof(ITestInterface), typeof(SampleProxy), "coreproxy2");
            poxy2.Write("here was invoked"); //---> "here was invoked by coreproxy2"

            var poxy3 = ProxyGenerator.Create<ITestInterface, SampleProxy>("coreproxy3");
            poxy3.Write("here was invoked"); //---> "here was invoked by coreproxy3"
        
            var poxy4 = ProxyGenerator.Create<ITestInterface>(new SampleProxy("coreproxy4"));
            poxy4.Write("here was invoked"); //---> "here was invoked by coreproxy4"
        }
    }

    public class SampleProxy : IInterceptor
    {
        private string proxyName { get; }

        public SampleProxy(string name)
        {
            this.proxyName = name;
        }

        public object Intercept(MethodInfo method, object[] parameters)
        {
            Console.WriteLine(parameters[0] + " by " + proxyName);
            return null;
        }

        public object Intercept(InvocationContext invocationContext, object[] parameters)
        {
            Console.WriteLine(parameters[0] + " by " + proxyName);
            return null;
        }
    }

    public interface ITestInterface
    {
        void Write(string writesome);
    }

    public class TestInterface : ITestInterface
    {
        public void Write(string writesome)
        {
            Console.Write("TestInterface write");
        }
    }
}