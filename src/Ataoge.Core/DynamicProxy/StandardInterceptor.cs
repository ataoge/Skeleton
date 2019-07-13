using System.Reflection;

namespace Ataoge.DynamicProxy
{
    public class StandardInterceptor : IInterceptor
    {
        protected virtual void PreProceed(InvocationContext invocationContext, object[] parameters){} 
        protected virtual void PostProceed(InvocationContext invocationContext, ref object returnValue, params object[] args){} 

        public object Intercept(InvocationContext invocationContext, object[] parameters)
        {
            PreProceed(invocationContext, parameters); 
            //object retValue = invocation.Proceed( args ); 
            object retValue = invocationContext.ImplementationMethod.Invoke(invocationContext.Implementation, parameters);
            PostProceed(invocationContext, ref retValue, parameters); 
            return retValue; 
         
        }
    }
}