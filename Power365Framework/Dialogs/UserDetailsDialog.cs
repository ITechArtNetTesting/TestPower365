using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class UserDetailsDialog : ModalDialogBase
    {
        public ButtonElement RefreshButton
        {
            get
            {
                var refreshButton = By.XPath(string.Format(_actionButtonFormat, "refresh"));
                return new ButtonElement(refreshButton, WebDriver);
            }
        }

        public ButtonElement SyncButton
        {
            get
            {
                var syncButton = By.XPath(string.Format(_actionButtonFormat, "sync"));
                return new ButtonElement(syncButton, WebDriver);
            }
        }

        public ButtonElement StopButton
        {
            get
            {
                var stopButton = By.XPath(string.Format(_actionButtonFormat, "stop"));
                return new ButtonElement(stopButton, WebDriver);
            }
        }

        public ButtonElement PrepareButton
        {
            get
            {
                var prepareButton = By.XPath(string.Format(_actionButtonFormat, "prepare"));
                return new ButtonElement(prepareButton, WebDriver);
            }
        }

        public ButtonElement RollbackButton
        {
            get
            {
                var rollbackButton = By.XPath(string.Format(_actionButtonFormat, "rollback"));
                return new ButtonElement(rollbackButton, WebDriver);
            }
        }

        public ButtonElement CutoverButton
        {
            get
            {
                var cutoverButton = By.XPath(string.Format(_actionButtonFormat, "cutover"));
                return new ButtonElement(cutoverButton, WebDriver);
            }
        }

        public ButtonElement CloseButton
        {
            get
            {
                return new ButtonElement(_closeButton, WebDriver);
            }
        }

        public TableElement JobsTable
        {
            get
            {
                return new TableElement(_jobsTable, WebDriver);
            }
        }

        private readonly By _closeButton = By.XPath("//*[contains(@data-translation, 'Close')]");
        private readonly By _jobsTable = By.XPath("//div[contains(@class, 'modal in')]//*[contains(@class, ' table-frame')]");
        private readonly By _detailsState = By.XPath("//span[contains(@data-bind,'State')]");

        private readonly string _actionButtonFormat = "//div[contains(@class, 'modal in')]//*[contains(@data-bind, '{0}')]";
        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";
        private readonly string _migrationStateTextLocatorFormat = "//*[contains(@data-bind, 'migrationState')][contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]";

        public const string DOWNLOAD_LOGS = "//ancestor::tr//*[contains(@class,'fa-download')]";

        public UserDetailsDialog(IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(UserDetailsDialog)), webDriver) { }

        public T PerformAction<T>(ActionType action)
          where T : ModalDialogBase     
        {
            ButtonElement button = null;
            switch (action)
            {
                case ActionType.Sync:
                    button = SyncButton;
                    break;
                case ActionType.Stop:
                    button = StopButton;
                    break;
                case ActionType.Prepare:
                    button = PrepareButton;
                    break;
                case ActionType.Cutover:
                    button = CutoverButton;
                    break;
                case ActionType.Rollback:
                    button = RollbackButton;
                    break;
                default:
                    throw new Exception("Invalid action for this dialog.");
            }

            return button.ClickModal<T>();
        }

        public void UsersValidateState(string entry, StateType state, int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            var value = state.GetDisplay();

            var rowEntryTextValue = string.Format(_migrationStateTextLocatorFormat, value.ToLower());
            var stateLocator = By.XPath(rowEntryTextValue);
            
            if (!IsElementVisible(stateLocator, timeoutInSec, pollIntervalInSec, () => RefreshButton.Click()))
                throw new Exception(string.Format("Entry of '{0}' with state '{1}' was not found.", entry, value));
        }
    }
}
