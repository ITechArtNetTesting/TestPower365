using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.Waiters;

namespace TestFramework.Interceptors
{
    public class PageLoggingInterceptor: IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            bool ExecutedWithoutExeptions = false;
            while (ExecutedWithoutExeptions == false) 
            {
                try
                {
                    WebDriverWaiter.WaitForJSLoad();
                    invocation.Proceed();
                    ExecutedWithoutExeptions = true;                    
                }
                catch (Exception e)
                {
                    //Exception                    
                }
                finally
                {
                    //Exiting Method
                }
            }            
        }
    }
}
