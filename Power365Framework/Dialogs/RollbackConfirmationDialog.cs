using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class RollbackConfirmationDialog : ModalDialogBase
    {
        private readonly By _rollbackResetPermissionsLabelYes = By.XPath("//label[contains(@for, 'resetPermissions')]");
        private readonly By _rollbackResetPermissionsLabelNo = By.XPath("//label[contains(@for, 'dontResetPermissions')]");
        private readonly By _rollbackResetPermissionsRadioYes = By.Id("resetPermissions");
        private readonly By _rollbackResetPermissionsRadioNo = By.Id("dontResetPermissions");
        private readonly By _rollbackSureLabel = By.XPath("//label[contains(@for, 'rollbackCheckbox')]");
        private readonly By _rollbackSureCheckbox = By.Id("rollbackCheckbox");
        private readonly By _rollbackConfirmButton = By.XPath("//button[contains(@data-bind, 'rollback') and not(@disabled='')]");

        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";

        public RollbackConfirmationDialog(IWebDriver webDriver) 
            : base(webDriver) { }

        public void Yes(bool resetPermissions = true)
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

        public void No()
        {
            var noButton = By.XPath(string.Format(_confirmationDialogButtonFormat, "No"));
            ClickElementBy(noButton);
        }
    }
}