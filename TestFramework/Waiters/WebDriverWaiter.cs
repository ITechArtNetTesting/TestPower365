using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using T365.Framework;

namespace TestFramework.Waiters
{
    public static class WebDriverWaiter
    {
        public static void WaitForElementIsClickable(IWebElement element)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
            wait.Until(ExpectedConditions.ElementToBeClickable(element));
        }
        public static void WaitForDOMLoad()
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)Browser.GetDriver();
            WebDriverWait wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
            wait.Until(wd => (string)js.ExecuteScript("return document.readyState") == "complete");
        }
        public static void WaitForElementIsStalenessOf(IWebElement element)
        {
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
            wait.Until(ExpectedConditions.StalenessOf(element));
        }
        public static void Wait(int SecondsToWait)
        {
            var now = DateTime.Now;
            var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromSeconds(SecondsToWait));
            wait.PollingInterval = TimeSpan.FromMilliseconds(100);
            wait.Until(wd => (DateTime.Now - now) - TimeSpan.FromSeconds(SecondsToWait) > TimeSpan.Zero);
        }
        public static void WaitForAjaxLoad()
        {            
            if (jQueryExists())
            {
                var wait = new WebDriverWait(Browser.GetDriver(), TimeSpan.FromMinutes(1));
                wait.PollingInterval = TimeSpan.FromMilliseconds(100);
                wait.Until(wd => (bool)(Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
            }
        }
        static bool jQueryExists()
        {            
            try
            {
                (Browser.GetDriver() as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0");
                return true;
            }
            catch (Exception)
            {
                return false;                     
            }            
        }
    }
}
