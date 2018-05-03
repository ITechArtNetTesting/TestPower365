using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class O365SignInPage: PageBase
    {
        private static readonly By _locator = By.Name("f1");

        private readonly By _loginInput = By.Name("loginfmt");
        private readonly By _passwordInput = By.Name("passwd");
        private readonly By _primaryButton = By.ClassName("btn-primary");
        private readonly By _useAnotherAccountButton = By.Id("otherTile");
        private readonly By _dontShowAgain = By.Name("DontShowAgain");
        
        public O365SignInPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        public ProjectListPage SignIn(string username, string password)
        {
            var loginInputElement = FindVisibleElement(_loginInput);
            loginInputElement.SendKeys(username);
            
            ClickElementBy(_primaryButton);

            var passwordInputElement = FindVisibleElement(_passwordInput);
            passwordInputElement.SendKeys(password);

            ClickElementBy(_primaryButton);

            try
            {
                ClickElementBy(_dontShowAgain);
            }
            catch (Exception) { }

            return ClickElementBy<ProjectListPage>(_primaryButton);
        }

        public void AuthorizeTenant(string username, string password)
        {
            var loginInputElement = FindVisibleElement(_loginInput);
            loginInputElement.SendKeys(username);

            //Next
            ClickElementBy(_primaryButton);

            var passwordInputElement = FindVisibleElement(_passwordInput);
            passwordInputElement.SendKeys(password);

            //Signin
            ClickElementBy(_primaryButton);
            
            //Accept
            ClickElementBy(_primaryButton);
        }

    }
}
