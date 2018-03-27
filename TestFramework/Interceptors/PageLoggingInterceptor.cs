using Castle.DynamicProxy;
using System;
using TestFramework.Driver;
using TestFramework.PageObjects.BasePages;
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
                    SwitchDriver.ToWindow(((BasePage)invocation.InvocationTarget).GetPageWindow());
                    WebDriverWaiter.WaitForDOMLoad();
                    WebDriverWaiter.WaitForAjaxLoad();
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
