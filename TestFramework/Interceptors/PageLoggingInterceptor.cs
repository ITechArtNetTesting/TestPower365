using Castle.DynamicProxy;
using OpenQA.Selenium;
using System;
using System.IO;
using T365.Framework;
using TestFramework.Drivers;
using TestFramework.PageObjects.BasePages;
using TestFramework.Waiters;

namespace TestFramework.Interceptors
{
    public class PageLoggingInterceptor: IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {            
            bool ExecutedWithoutExeptions = false;
            while (ExecutedWithoutExeptions == false) 
            {
                try
                {
                    ((BasePage)invocation.InvocationTarget).SwitchWindow();                    
                    WebDriverWaiter.WaitForDOMLoad();
                    WebDriverWaiter.WaitForAjaxLoad();
                    Driver.TakeScreenShot(invocation.Method.Name, ((BasePage)invocation.InvocationTarget).GetType().Name, true);
                    invocation.Proceed();
                    Driver.TakeScreenShot(invocation.Method.Name, ((BasePage)invocation.InvocationTarget).GetType().Name,false);
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
