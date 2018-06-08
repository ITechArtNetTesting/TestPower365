using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class LandingPage: PageBase
    {
        private static readonly By _locator = By.Id("landingContainer");

        public LandingPage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(LandingPage)), _locator, webDriver) { }

        private readonly By _migrateAndIntegrateButton = By.XPath("//div[contains(@class,'power365')]");

        //public LandingPage ClickMigrateAndIntegrateButton()
        //{
        //    return ClickElementBy<LandingPage>(_migrateAndIntegrateButton,10,1);
        //}

        public ProjectListPage ClickMigrateAndIntegrateButton()
        {
            return ClickElementBy<ProjectListPage>(_migrateAndIntegrateButton, 10, 1);
        }
    }
}
