using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class HomePage: PageBase
    {
        private static readonly By _locator = By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]");

        private readonly By _signInButton = By.XPath("//a[contains(@href, 'SignIn')][contains(@class, 'btn')]");

        public HomePage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(HomePage)), _locator, webDriver) { }

        public O365SignInPage ClickSignIn()
        {
            return ClickElementBy<O365SignInPage>(_signInButton);
        }
    }
}
