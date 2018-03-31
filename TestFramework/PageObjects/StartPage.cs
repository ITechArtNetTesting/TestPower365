using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System.Collections.Generic;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;

namespace TestFramework.PageObjects
{
    public class StartPage: BasePage,IStartPage
    {       
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='SignIn']")]
        IWebElement SignInButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='hamburger']")]
        IWebElement OpenRightBarButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/ul[@class='topnav-menu']")]
        IWebElement P365Logo { get; set; }

        public void ClickOnLogo()
        {
            Perform.Click(P365Logo);
        }

        public void ClickOpenRightBarButton()
        {
            Perform.Click(OpenRightBarButton);            
        }

        public void ClickSignIn()
        {            
            Perform.Click(SignInButton);
        }
    }
}
