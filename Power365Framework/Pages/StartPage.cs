using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class StartPage:PageBase
    {
        private static readonly By _locator = By.Id("landingContainer");

        public StartPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        private readonly By _migrateAndIntegrateButton = By.XPath("//div[contains(@class,'power365')]");

        public ProjectListPage ClickMigrateAndIntegrateButton()
        {
            return ClickElementBy<ProjectListPage>(_migrateAndIntegrateButton,10,1);
        }
    }
}
