using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Interceptors
{
    public class StepsLoggingInterceptor : IInterceptor
    {        
        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;            

            try
            {
                //Entered Method
                invocation.Proceed();
                
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
