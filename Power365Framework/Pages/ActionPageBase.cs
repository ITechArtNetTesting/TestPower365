using BinaryTree.Power365.AutomationFramework.Dialogs;
using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using log4net;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public abstract class ActionPageBase : PageBase
    {
        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";
        private readonly string _actionDropdownSelectionFormat = "//button[contains(@aria-expanded, 'true')]/ancestor::div[contains(@class, 'dropdown')]//ul[contains(@class, 'dropdown-menu')]//li[contains(@data-toggle, 'tooltip')]//*[contains(text(), '{0}')]";
        private readonly string _tabPaneLocator = "//div[contains(@class,'tab-pane active')]";

        private readonly By _actionsDropdown;
        private readonly By _actionsDropdownExpanded;

        private readonly By _applyActionButtonEnabled;
        private readonly By _applyActionButtonDisabled;

        private readonly By _rollbackResetPermissionsLabelYes = By.XPath("//label[contains(@for, 'resetPermissions')]");
        private readonly By _rollbackResetPermissionsLabelNo = By.XPath("//label[contains(@for, 'dontResetPermissions')]");
        private readonly By _rollbackResetPermissionsRadioYes = By.Id("resetPermissions");
        private readonly By _rollbackResetPermissionsRadioNo = By.Id("dontResetPermissions");
        private readonly By _rollbackSureLabel = By.XPath("//label[contains(@for, 'rollbackCheckbox')]");
        private readonly By _rollbackSureCheckbox = By.Id("rollbackCheckbox");
        private readonly By _rollbackConfirmButton = By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]");

        protected ActionPageBase(ILog logger, By locator, IWebDriver webDriver)
            : base(logger, locator, webDriver)
        {
            var actionsDropdownLocator = "//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]";
            var actionsDropdownExpanededLocator = "//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')][contains(@aria-expanded, 'true')]";
            var applyActionsButtonEnabledLocator = "//button[contains(@data-bind, 'applyAction')][not(@disabled='')]";
            var applyActionsButtonDisabledLocator = "//button[contains(@data-bind, 'applyAction')][@disabled='']";

            if (IsElementVisible(By.XPath(_tabPaneLocator), 0))
            {
                actionsDropdownLocator = _tabPaneLocator + actionsDropdownLocator;
                actionsDropdownExpanededLocator = _tabPaneLocator + actionsDropdownExpanededLocator;
                applyActionsButtonEnabledLocator = _tabPaneLocator + applyActionsButtonEnabledLocator;
                applyActionsButtonDisabledLocator = _tabPaneLocator + applyActionsButtonDisabledLocator;
            }

            _actionsDropdown = By.XPath(actionsDropdownLocator);
            _actionsDropdownExpanded = By.XPath(actionsDropdownExpanededLocator);
            _applyActionButtonEnabled = By.XPath(applyActionsButtonEnabledLocator);
            _applyActionButtonDisabled = By.XPath(applyActionsButtonDisabledLocator);
        }

        public void PerformAction(ActionType action)
        {
            DropdownSelectionAction(action);          
            if (!IsElementVisible(_applyActionButtonEnabled))
              throw new Exception("Could not find enabled Apply Action button.");     
                
            ClickElementBy(_applyActionButtonEnabled);
        }

        public T PerformAction<T>(ActionType action)
            where T : ModalDialogBase
        {
            DropdownSelectionAction(action);
            if (!IsElementVisible(_applyActionButtonEnabled))
                throw new Exception("Could not find enabled Apply Action button.");
            
            return ClickModalElementBy<T>(_applyActionButtonEnabled);
        }

        public void ConfirmAction(bool isYes = true)
        {
            var confirmDialogButton = By.XPath(string.Format(_confirmationDialogButtonFormat, isYes ? "Yes" : "No"));
            ClickElementBy(confirmDialogButton);
        }

        public void ConfirmRollback(bool resetPermissions = true)
        {
            if (resetPermissions)
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

        public void DropdownSelectionAction(ActionType action)
        {
            ClickElementBy(_actionsDropdown);

            if (!IsElementVisible(_actionsDropdownExpanded))
                throw new Exception("Could not find expanded Actions dropdown.");

            var actionDropdownSelection = By.XPath(string.Format(_actionDropdownSelectionFormat, action.GetDescription()));
            ClickElementBy(actionDropdownSelection);
        }

        public bool IsActionEnabled(ActionType action)
        {
            DropdownSelectionAction(action);
            return IsElementVisible(_applyActionButtonEnabled, 1);
        }

        public Element GetActionsDropdown()
        {
            return new Element(_actionsDropdown, WebDriver);
        }
    }
}
