using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree.Power365.AutomationFramework.Enums;
using BinaryTree.Power365.AutomationFramework.Extensions;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class UsersDetailsPage : PageBase
    {
        private static readonly By _locator = By.XPath("//div[@class='modal in']/*[@class='modal-dialog modal-lg']");

        public UsersDetailsPage(IWebDriver webDriver) : base(_locator, webDriver){}

        private readonly By _detailsSyncButton = By.XPath("//div[contains(@class, 'modal in')]//*[contains(@data-bind, 'sync')]");
        private readonly By _closeDetailsWindowButton = By.XPath("//div[contains(@class, 'modal in')]//div[contains(@class, 'modal-lg')]//button[contains(@data-dismiss, 'modal')][contains(@class, 'btn')]");
        private readonly By _refreshDetailsWindowButton = By.XPath("//button[contains(@data-bind, 'refresh.run')][not(@disabled='')]");
        private readonly By _detailsState = By.XPath("//span[contains(@data-bind,'State')]");

        public void CloseDetailsWindow()
        {
            ClickElementBy(_closeDetailsWindowButton);
        }

        public void PerformSyncFromDetails()
        {
            ClickElementBy(_detailsSyncButton);
        }

        public bool WaitForState_DetailPage(string entry, StateType state, int timeout = 5000, int pollIntervalSec = 0)
        {
            var value = state.GetDescription();
            if (state.GetDescription().ToLower() == "synced")
                value = "complete";

            Func<IWebDriver, bool> waitState = new Func<IWebDriver, bool>((IWebDriver ele) =>
            {
                return ele.FindElement(_detailsState).Text.ToLower().Contains(value.ToLower());
            });

            return EvaluateElement(_detailsState, waitState, () => ClickElementBy(_refreshDetailsWindowButton), timeout, pollIntervalSec);

        }
    }
}
