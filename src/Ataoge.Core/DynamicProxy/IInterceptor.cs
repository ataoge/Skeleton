using System.Reflection;

namespace Ataoge.DynamicProxy
{
    /// <summary>
    /// https://www.jb51.net/article/153783.htm
    /// dotnet add package System.Reflection.DispatchProxy --version 4.5.1
    /// </summary>
    public interface IInterceptor
    {
        /// <summary>
        /// 拦截器调用
        /// </summary>
        /// <param name="target">代理实例</param>
        /// <param name="method">所拦截的方法</param>
        /// <param name="parameters">所拦截方法传入的参数值</param>
        /// <returns>返回值会传递给方法返回值</returns> 
        //object Intercept(MethodInfo method, object[] parameters);
        object Intercept(InvocationContext invocationContext, object[] parameters);
    }

}