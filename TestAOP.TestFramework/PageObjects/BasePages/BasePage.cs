using OpenQA.Selenium.Support.PageObjects;
using System;
using T365Framework;
using TestFramework.Actions;
using TestFramework.Waiters;

namespace TestFramework.PageObjects.BasePages
{
    public abstract class BasePage
    {
        protected IActions Performe = new ActionsWithWait();
        public BasePage()
        {            
            PageFactory.InitElements(Browser.GetDriver(), this);         
        }
    }
}
