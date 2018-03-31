using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;

namespace TestFramework.PageObjects
{
    public class MicrosoftLoginPageWindow : BasePage, IMicrosoftLoginPageWindow
    {
        [FindsBy(How = How.Id, Using = "otherTileText")]
        IWebElement UseAnotherAccountButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/input[@type='email']")]
        IWebElement EmailInput { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/input[@type='submit']")]
        IWebElement NextButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/input[@name='passwd']")]
        IWebElement PasswordInput { get; set; }

        public void ClickNextButton()
        {
            Perform.Click(NextButton);
        }

        public void ClickUseAnotherAccount()
        {
            Perform.Click(UseAnotherAccountButton);
        }

        public void SendKeysToEmailInput(string keys)
        {
            Perform.SendKeys(EmailInput, keys);
        }

        public void SendKeysToPasswordInput(string keys)
        {
            Perform.SendKeys(PasswordInput, keys);
        }
    }
}
