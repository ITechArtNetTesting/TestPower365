using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ManagePublicFoldersPage : PageBase
    {
        public TableElement PublicFolders
        {
            get
            {
                return new TableElement(_publicFoldersTable, WebDriver);
            }
        }

        private static readonly By _locator = By.XPath("//span[@data-translation='SelectMultiplePublicFolderMigrationToApplyActionsAndDoubleClick']");
        //
        //@@@ REQ:ID
        private readonly By _publicFoldersTable = By.XPath("//table[contains(@class, 'table-expanded')]");
        //@@@ REQ:ID
        private readonly By _actionsDropdown = By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");
        private readonly By _actionsDropdownExpanded = By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]");
        
        private readonly By _applyActionButtonEnabled = By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]");
        private readonly By _applyActionButtonDisabled = By.XPath("//button[contains(@data-bind, 'applyAction')][@disabled='']");

        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";

        private readonly string _actionDropdownSelectionFormat = "//button[contains(@aria-expanded, 'true')]/ancestor::div[contains(@class, 'dropdown')]//ul[contains(@class, 'dropdown-menu')]//li[contains(@data-toggle, 'tooltip')]//*[contains(text(), '{0}')]";
        
        public ManagePublicFoldersPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        public void PerformAction(ActionType action)
        {
            ClickElementBy(_actionsDropdown);
            
            if (!IsElementVisible(_actionsDropdownExpanded))
                throw new Exception("Could not find expanded Actions dropdown.");

            var actionDropdownSelection = By.XPath(string.Format(_actionDropdownSelectionFormat, action.GetDescription()));
            ClickElementBy(actionDropdownSelection);

            if (!IsElementVisible(_applyActionButtonEnabled))
                throw new Exception("Could not find enabled Apply Action button.");

            ClickElementBy(_applyActionButtonEnabled);
        }

        public void ConfirmAction(bool isYes = true)
        {
            var confirmDialogButton = By.XPath(string.Format(_confirmationDialogButtonFormat, isYes ? "Yes" : "No"));
            ClickElementBy(confirmDialogButton);
        }
        
        public bool IsPublicFolderState(string folder, StateType state, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            return PublicFolders.RowHasValue(folder, state.GetDisplay(), timeoutInSec, pollIntervalSec);
        }
    }
}