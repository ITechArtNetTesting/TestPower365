using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using OpenQA.Selenium;
using SeleniumExtras.WaitHelpers;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
     public class UserDetailsPage : PageBase
    {
        public UserDetailsPage(By locator, IWebDriver webDriver) : base(locator, webDriver)
        {
        }
        private static readonly By _locator = By.XPath("//div[@class='modal in']");
        //private static readonly By _locator = By.XPath("//div[@class='modal in']//span[text()='User Details']");

        public UserDetailsPage(IWebDriver webDriver) : base(_locator, webDriver) { }

        private readonly string detailsActions = "//div[contains(@class, 'modal in')]//*[contains(@data-bind, '{0}')]";
        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";

        private readonly By _closeDetailsWindowButton = By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'modal-lg')]//button[contains(@data-dismiss, 'modal')][contains(@class, 'btn')]");
        private readonly By _refreshDetailsWindowButton = By.XPath("//button[contains(@data-bind, 'refresh.run')][not(@disabled='')]");

        private readonly By _detailsState = By.XPath("//span[contains(@data-bind,'State')]");

        private string _migrationStateTextLocatorFormat = "//*[contains(@data-bind, 'migrationState')][contains(translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{0}')]";

        public void CloseDetailsWindow()
        {
            ClickElementBy(_closeDetailsWindowButton);
        }

        public void PerformAction(ActionType state)
        {
            By action = By.XPath(string.Format(detailsActions, state.GetDescription().ToLower()));
            ClickElementBy(action);
        }

        public void ConfirmAction(bool isYes = true)
        {
            var confirmDialogButton = By.XPath(string.Format(_confirmationDialogButtonFormat, isYes ? "Yes" : "No"));
            ClickElementBy(confirmDialogButton);
        }

        public void WaitForState_DetailPage(string entry, StateType state, int timeout = 5000, int pollIntervalSec = 0)
        {
           var value = state.GetDescription();
           if (state.GetDescription().ToLower() == "synced") value = "complete";
                    
            var rowEntryTextValue = string.Format(_migrationStateTextLocatorFormat, value.ToLower());
            var stateLocator = By.XPath(rowEntryTextValue);
           
            EvaluateElement(stateLocator, ExpectedConditions.ElementIsVisible(stateLocator), () => ClickElementBy(_refreshDetailsWindowButton), timeout, pollIntervalSec);

        }


    }
}
