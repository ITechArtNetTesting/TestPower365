using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Waiters;

namespace TestFramework.PageObjects
{
    class MicrosoftLoginPage : BasePage, IMicrosoftLoginPage
    {        
        [FindsBy(How = How.Id, Using = "idSIButton9")]
        IWebElement YesToStaySignedButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/input[@value='Sign in']")]
        IWebElement SignInButton { get; set; }


        [FindsBy(How = How.XPath, Using = "//*/input[@value='Next']")]
        IWebElement NextButton { get; set; }


        [FindsBy(How = How.Name, Using = "loginfmt")]
        IWebElement EmailPhoneOrSkypeInput { get; set; }


        [FindsBy(How = How.Name, Using = "passwd")]
        IWebElement PasswordInput { get; set; }

        public void ClickNextButton()
        {
            NextButton.Click();
        }

        public void ClickSignInButton()
        {
            SignInButton.Click();
        }

        public void ClickYesToStaySignedButton()
        {
            YesToStaySignedButton.Click();
        }

        public void SendKeysToEmailPhoneOrSkypeInput(string keys)
        {
            EmailPhoneOrSkypeInput.SendKeys(keys);
        }

        public void SendKeysToPasswordInput(string keys)
        {
            DefaultWaiter.WaitForElementIsDisplayed(PasswordInput);
            PasswordInput.SendKeys(keys);
        }
    }
}
