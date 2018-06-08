using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Workflows;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class MigrationProfilesPage : PageBase
    {
        private static By _locator = By.Id("migrationProfileContainer");
        private static By _editFrofilesLocator = By.Id("editMigrationProfileContainer");
        
        public MigrationProfilesPage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(MigrationProfilesPage)), _locator, webDriver) { }

       public TableElement ProfileTable
        {
            get
            {
                return new TableElement(_profileTable, WebDriver);
            }
        }

        private readonly By _profileTable = By.XPath("//table[contains(@class, 'table-expanded')]");
        private By _addProfile = By.XPath("//div[contains(@class, 'ibox-content')]//*[contains(text(), 'Add Profile')]");

        private readonly string _actionProfileFormat = "//tr[.//*[contains(text(), '{0}')]]//div[contains(@class, 'table-actions')]//*[contains(text(), '{1}')]";
        private readonly string _profileFormat = "//tr[.//* [contains(text(), '{0}')]]";


        public bool IsDeleteLinkVisible(string profileName)
        {
            HoverOverProfileLabel(profileName);
            var profileAction = By.XPath(string.Format(_actionProfileFormat, profileName,"Delete"));
            return IsElementVisible(profileAction);
        }
        
        public void HoverOverProfileLabel(string label)
        {
            var profileLocator = By.XPath(string.Format(_profileFormat, label));
            HoverElement(profileLocator);
        }

        public MigrationProfileWorkflow ClickAddNewProfile()
        {
            ClickElementBy(_addProfile);
            return new MigrationProfileWorkflow(this, _editFrofilesLocator, WebDriver);           
        }
    }
}
