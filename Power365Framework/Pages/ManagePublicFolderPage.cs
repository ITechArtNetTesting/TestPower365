using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using BinaryTree.Power365.AutomationFramework.Workflows;
using log4net;
using OpenQA.Selenium;
using System.Linq;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ManagePublicFoldersPage : ActionPageBase
    {
        public TableElement PublicFolders
        {
            get
            {
                return new TableElement(_publicFoldersTable, WebDriver);
            }
        }

        private static readonly By _locator = By.XPath("//span[@data-translation='SelectMultiplePublicFolderMigrationToApplyActionsAndDoubleClick']");

        private readonly By _addPublicFolderMigration = By.XPath("//span[@data-translation='AddPublicFolderMigration']");
        private readonly By _publicFoldersTable = By.XPath("//table[contains(@class, 'table-expanded')]");
      
        public ManagePublicFoldersPage(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(ManagePublicFoldersPage)), _locator, webDriver) { }

        public AddPublicFolderWorkflow AddPublicFolderMigration()
        {
            ClickElementBy(_addPublicFolderMigration);
            return new AddPublicFolderWorkflow(this, WebDriver);
        }
       
        public bool IsPublicFolderState(string folder, StateType state, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            return PublicFolders.RowHasValue(folder, state.GetDisplay(), timeoutInSec, pollIntervalSec);
        }

        public bool IsPublicFolderAnyState(string folder, StateType[] states, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            var stateValues = states.Select(s => s.GetDisplay()).ToArray();

            return PublicFolders.RowHasAnyValue(folder, stateValues, timeoutInSec, pollIntervalSec);
        }
    }
}