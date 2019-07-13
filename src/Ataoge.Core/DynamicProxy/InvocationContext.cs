using System.Reflection;

namespace Ataoge.DynamicProxy
{
    public class InvocationContext
    {
        public MethodInfo ImplementationMethod {get; set;}

        public object Implementation {get; set;}

        public object[] Parameters {get; set;}

        public object Proxy {get; set;}
    }
}