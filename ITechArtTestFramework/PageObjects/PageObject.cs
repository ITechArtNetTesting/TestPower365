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
    }
}
