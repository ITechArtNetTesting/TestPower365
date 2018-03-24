using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;

namespace TestFramework.PageObjects
{
    public class StartPage: BasePage,IStartPage
    {       
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='SignIn']")]
        IWebElement SignInButton { get; set; }

        public void ClickSignIn()
        {
            SignInButton.Click();
        }
    }
}
