using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class MigrationProfilesPage : PageBase
    {
        private static By _locator = By.Id("migrationProfileContainer");       

        public MigrationProfilesPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        private readonly string _actionProfileFormat = "//tr[.//*[contains(text(), '{0}')]]//div[contains(@class, 'table-actions')]//*[contains(text(), '{1}')]";
        private readonly string _profileFormat = "//tr[.//* [contains(text(), '{0}')]]";
        private By _addProfile = By.XPath("//div[contains(@class, 'ibox-content')]//*[contains(text(), 'Add Profile')]");

        public bool IsDeleteLinkVisible(string profileName)
        {
            HoverOverProfileLabel(profileName);
            var profileAction = By.XPath(String.Format(_actionProfileFormat, profileName,"Delete"));
            return IsElementVisible(profileAction);
        }
                

        public void HoverOverProfileLabel(string label)
        {
            var profileLocator = By.XPath(String.Format(_profileFormat, label));
            HoverElement(profileLocator);
        }

        public EditMigrationProfilePage ClickAddNewProfile(string profileName)
        {
          return  ClickElementBy<EditMigrationProfilePage>(_addProfile);  
        }
    }
}
