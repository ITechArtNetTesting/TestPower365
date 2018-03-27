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
            Performe.Click(NextButton);
        }

        public void ClickSignInButton()
        {            
            Performe.Click(SignInButton);
        }

        public void ClickYesToStaySignedButton()
        {            
            Performe.Click(YesToStaySignedButton);
        }

        public void SendKeysToEmailPhoneOrSkypeInput(string keys)
        {            
            Performe.SendKeys(EmailPhoneOrSkypeInput, keys);
        }

        public void SendKeysToPasswordInput(string keys)
        {
            Performe.SendKeys(PasswordInput, keys);
        }
    }
}
