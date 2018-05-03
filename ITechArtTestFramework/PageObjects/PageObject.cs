using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using TP365Framework.Drivers;

namespace TP365Framework.PageObjects
{
    public abstract class PageObject
    {        
        public Driver driver { get; set; }

        public void InitWebelements(Driver driver)
        {
            this.driver = driver;           
            PageFactory.InitElements(driver.GetDriver, this);            
        }
        public void WaitElementShows(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromSeconds(10);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);

            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                return ele.Displayed;
            });
            wait.Until(waiter);
        }
    }
}
