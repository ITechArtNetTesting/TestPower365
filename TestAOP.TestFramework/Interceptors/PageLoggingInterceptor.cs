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
            try
            {
                WebDriverWaiter.WaitForJSLoad();
                invocation.Proceed();
                //Sucessfully executed method
            }
            catch (Exception e)
            {
                //Exception
                throw;
            }
            finally
            {
                //Exiting Method
            }
        }
    }
}
