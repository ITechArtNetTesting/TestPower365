using OpenQA.Selenium.Support.PageObjects;
using System;
using T365.Framework;
using TestFramework.Actions;
using TestFramework.Drivers;
using TestFramework.Waiters;

namespace TestFramework.PageObjects.BasePages
{
    public abstract class BasePage
    {
        protected string PageWindow;
        protected IActions Perform = new Actions.Actions();
        public BasePage()
        {               
            PageFactory.InitElements(Browser.GetDriver(), this);               
        }        
        public void SwitchWindow()
        {
            if (PageWindow == null)
            {
                PageWindow = Browser.GetDriver().WindowHandles[Browser.GetDriver().WindowHandles.Count - 1];
            }
            else
            {
                if (!Browser.GetDriver().WindowHandles.Contains(PageWindow))
                {
                    PageWindow = Browser.GetDriver().WindowHandles[Browser.GetDriver().WindowHandles.Count - 1];
                }
            }
            Drivers.Driver.SwitchWindowTo(PageWindow);
        }
        protected void UpdateElements()
        {
            PageFactory.InitElements(Browser.GetDriver(), this);
        }
    }
}
