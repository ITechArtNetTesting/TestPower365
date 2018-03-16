using ITechArtTestFramework.PageObjects;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Pages
{
    class LoginPage : PageObject
    {
        [FindsBy(How = How.XPath, Using = "//a[contains(@href, 'SignIn')][contains(@class, 'btn')]")]
        IWebElement signInButton { get; set; }


        [FindsBy(How = How.Id, Using = "i0116")]
        IWebElement loginTextBox { get; set; }

        [FindsBy(How = How.Id, Using = "idSIButton9")]
        IWebElement next { get; set; }

        [FindsBy(How = How.Id, Using = "i0118")]
        public IWebElement passwordInputText { get; set; }

        public void ClickSignIn()
        {
            signInButton.Click();
        }

        public void SetLogin(string login)
        {
            loginTextBox.SendKeys(login);
        }

        public void NextClick()
        {
            next.Click();
        }

        public void SetPassword(string password)
        {           
            passwordInputText.SendKeys(password);
        }
    }
}
