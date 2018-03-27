using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using TestFramework.Waiters;

namespace TestFramework.Actions
{
    public class ActionsWithWait : IActions
    {
        public void Click(IWebElement element)
        {            
            DefaultWaiter.WaitForElementIsDisplayed(element);
            DefaultWaiter.WaitForElementIsEnabled(element);
            DefaultWaiter.WaitForElementIsVisible(element);
            WebDriverWaiter.WaitForElementIsClickable(element);            
            element.Click();            
        }

        public void Click(IWebElement element, int delay)
        {         
            DefaultWaiter.WaitForElementIsDisplayed(element);
            DefaultWaiter.WaitForElementIsEnabled(element);
            DefaultWaiter.WaitForElementIsVisible(element);
            WebDriverWaiter.WaitForElementIsClickable(element);
            WebDriverWaiter.Wait(delay);
            element.Click();
        }

        public void SendKeys(IWebElement element, string keys)
        {         
            DefaultWaiter.WaitForElementIsDisplayed(element);
            DefaultWaiter.WaitForElementIsEnabled(element);
            DefaultWaiter.WaitForElementIsVisible(element);
            WebDriverWaiter.WaitForElementIsClickable(element);            
            element.SendKeys(keys);            
        }

        public void SendKeys(IWebElement element, string keys, int delay)
        {         
            DefaultWaiter.WaitForElementIsDisplayed(element);
            DefaultWaiter.WaitForElementIsEnabled(element);
            DefaultWaiter.WaitForElementIsVisible(element);
            WebDriverWaiter.WaitForElementIsClickable(element);
            WebDriverWaiter.Wait(delay);
            element.SendKeys(keys);
        }
    }
}
