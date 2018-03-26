using OpenQA.Selenium.Support.PageObjects;
using System;
using T365Framework;
using TestFramework.Waiters;

namespace TestFramework.PageObjects.BasePages
{
    public abstract class BasePage
    {
        public BasePage()
        {            
            PageFactory.InitElements(Browser.GetDriver(), this);         
        }
    }
}
