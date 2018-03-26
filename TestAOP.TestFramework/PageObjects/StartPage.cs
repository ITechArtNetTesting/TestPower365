using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Waiters;

namespace TestFramework.PageObjects
{
    public class StartPage: BasePage,IStartPage
    {       
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='SignIn']")]
        IWebElement SignInButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*[@id='hamburger']")]
        IWebElement OpenRightBarButton { get; set; }

        public void ClickOpenRightBarButton()
        {
            WebDriverWaiter.WaitForJSLoad();
            OpenRightBarButton.Click();
        }

        public void ClickSignIn()
        {
            SignInButton.Click();
        }
    }
}
