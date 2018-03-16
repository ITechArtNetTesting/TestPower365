using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using ITechArtTestFramework.Drivers;
using ITechArtTestFramework.StaticClasses;

namespace ITechArtTestFramework.PageObjects
{
    public abstract class PageObject
    {        
        public Driver driver { get; set; }

        public void InitWebelements(Driver driver)
        {
            this.driver = driver;           
            PageFactory.InitElements(driver.GetDriver, this);            
        }

        public void Wait(IWebElement element)
        {
            DefaultWait<IWebElement> wait = new DefaultWait<IWebElement>(element);
            wait.Timeout = TimeSpan.FromMinutes(2);
            wait.PollingInterval = TimeSpan.FromMilliseconds(250);

            Func<IWebElement, bool> waiter = new Func<IWebElement, bool>((IWebElement ele) =>
            {
                return ele.Enabled;
            });
            wait.Until(waiter);
        }
    }
}
