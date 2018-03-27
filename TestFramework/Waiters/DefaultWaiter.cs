using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestFramework.Waiters
{
    public static class DefaultWaiter
    {
        public static void WaitForElementIsDisplayed(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                try
                {
                    return ele.Displayed;
                }
                catch (Exception) { throw; }
            });
            wait.Until(waiter);
        }
        public static void WaitElementDisapear(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                try
                {
                    return !ele.Displayed;
                }
                catch (Exception) { throw; }
            });
            wait.Until(waiter);
        }
        public static void WaitForElementIsEnabled(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                try
                {
                    return ele.Enabled;
                }
                catch (Exception) { throw; }
            });
            wait.Until(waiter);
        }
        public static void WaitForElementIsVisible(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromMinutes(1);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);
            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                try
                {
                    return ele.Enabled && ele.Displayed;
                }
                catch (Exception) { throw; }
            });
            wait.Until(waiter);
        }       
    }
}
