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
            Perform.Click(NextButton);
        }

        public void ClickSignInButton()
        {            
            Perform.Click(SignInButton);
        }

        public void ClickYesToStaySignedButton()
        {            
            Perform.Click(YesToStaySignedButton);
        }

        public void SendKeysToEmailPhoneOrSkypeInput(string keys)
        {            
            Perform.SendKeys(EmailPhoneOrSkypeInput, keys);
        }

        public void SendKeysToPasswordInput(string keys)
        {
            Perform.SendKeys(PasswordInput, keys);
        }
    }
}
