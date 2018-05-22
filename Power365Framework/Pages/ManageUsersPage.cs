using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ManageUsersPage : PageBase
    {
        public TableElement Users
        {
            get
            {
                return new TableElement(_usersTable, WebDriver);
            }
        }

        public TableElement Waves
        {
            get
            {
                return new TableElement(_wavesTable, WebDriver);
            }
        }
        private static readonly By _locator = By.Id("manageUsersContainer");

        //@@@ REQ:ID
        private readonly By _usersTable = By.XPath("//div[contains(@id, 'users')]//table[contains(@class, 'table-expanded')]");
        private readonly By _wavesTable = By.XPath("//div[contains(@id, 'waves')]//table[contains(@class, 'table-expanded')]");
        //@@@ REQ:ID
        private readonly By _actionsDropdown = By.XPath("//div[@id='users']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]");
        private readonly By _actionsDropdownExpanded = By.XPath("//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]");

        private readonly By _rollbackResetPermissionsLabelYes = By.XPath("//label[contains(@for, 'resetPermissions')]");
        private readonly By _rollbackResetPermissionsLabelNo = By.XPath("//label[contains(@for, 'dontResetPermissions')]");
        private readonly By _rollbackResetPermissionsRadioYes = By.Id("resetPermissions");
        private readonly By _rollbackResetPermissionsRadioNo = By.Id("dontResetPermissions");
        private readonly By _rollbackSureLabel = By.XPath("//label[contains(@for, 'rollbackCheckbox')]");
        private readonly By _rollbackSureCheckbox = By.Id("rollbackCheckbox");
        private readonly By _rollbackConfirmButton = By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]");

        private readonly By _applyActionButtonEnabled = By.XPath("//button[contains(@data-bind, 'applyAction')][not(@disabled='')]");
        private readonly By _applyActionButtonDisabled = By.XPath("//button[contains(@data-bind, 'applyAction')][@disabled='']");

        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";

        private readonly string _actionDropdownSelectionFormat = "//button[contains(@aria-expanded, 'true')]/ancestor::div[contains(@class, 'dropdown')]//ul[contains(@class, 'dropdown-menu')]//li[contains(@data-toggle, 'tooltip')]//*[contains(text(), '{0}')]";

        private readonly By _newMigrationWaveButton = By.XPath("//span[contains(@data-bind,'addWave')]");

        private readonly By _migrationWaveTab = By.XPath("//a[contains(@href,'waves')]//span");
        private readonly By _usersTab = By.XPath("//a[contains(@href,'users')]//span");

        private readonly By _Tab = By.XPath("//*[contains(@class,'nav nav-tabs')]/li");

        private readonly By _searchInput = By.XPath("//div[contains(@class, 'search-group')]//input"); 
        private readonly By _searchButton = By.XPath("//i[contains(@class,'search')]");

        //wave
        private readonly string _waveNameModalWindow = "//*[contains(@class,'modal-dialog')]//div[contains(@class, 'radio')]//span[contains(text(), '{0}')]";
        private readonly By _okButtonModalWindow = By.XPath("//*[contains(@class,'modal-footer')]//span[contains(text(), 'Ok')]");


        public ManageUsersPage(IWebDriver webDriver)
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

        public void ConfirmRollback(bool resetPermissions = true)
        {
            if(resetPermissions)
            {
                ClickElementBy(_rollbackResetPermissionsLabelYes);

                if (!IsElementSelectedState(_rollbackResetPermissionsRadioYes, true))
                    throw new Exception("Rollback Reset Permission Radio Button is not Selected and should be.");
            }
            else
            {
                ClickElementBy(_rollbackResetPermissionsLabelNo);

                if (!IsElementSelectedState(_rollbackResetPermissionsRadioNo, true))
                    throw new Exception("Rollback Reset Permission Radio Button is not Selected and should be.");
            }

            ClickElementBy(_rollbackSureCheckbox);

            if (!IsElementSelectedState(_rollbackSureCheckbox, true))
                throw new Exception("Rollback Sure Checkbox should be checked but is not");

            ClickElementBy(_rollbackConfirmButton);
        }

        public bool IsUserState(string user, StateType state, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            return Users.RowHasValue(user, state.GetDisplay(), timeoutInSec, pollIntervalSec);
        }
        public PageElement GetActionsDropdown()
        {           
            return new PageElement(_actionsDropdown, WebDriver);
        }

        public void SwichToMigrationWavesTab()
        {
            ClickElementBy(_migrationWaveTab,5);            
        }

        public void SwichToUsersTab()
        {
            ClickElementBy(_usersTab, 5);
        }

        public bool CheckMigrationWavesTabOpen()
        {
            return IsElementVisible(_newMigrationWaveButton);
        }

        public bool MigrationWaveTabIsVisible()
        {
            return IsElementVisible(_migrationWaveTab);
        }

        public EditProjectPage clickNewWaveButton()
        {
            return ClickElementBy<EditProjectPage>(_newMigrationWaveButton, 5);
        }

        public UserDetailsPage OpenDetailsOf(String user)
        {
            return DoubleClickElementBy<UserDetailsPage>(Users.GetRowLocatorByValue(user));              
        }

        public void PerformSearch(string searchWord)
        {
        var searchInput = FindVisibleElement(_searchInput);
            searchInput.SendKeys(searchWord);           
            ClickElementBy(_searchButton); 
        }

       public void SelectWave(string waveName)
        {
            var waveNameModalWindow = By.XPath(string.Format(_waveNameModalWindow, waveName));
            ClickElementBy(waveNameModalWindow);
            ClickElementBy(_okButtonModalWindow);
        }

        public void SelectWave()
        {
            //create
        }

    }
}

