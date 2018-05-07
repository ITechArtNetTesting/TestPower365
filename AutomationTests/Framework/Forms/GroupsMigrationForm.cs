using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using OpenQA.Selenium;
using Product.Framework.Elements;
using Product.Framework.Enums;

namespace Product.Framework.Forms
{
	public class GroupsMigrationForm : UsersForm
	{
        private static readonly By TitleLocator = By.XPath("//a[contains(@data-bind, 'GroupMigrationsDialog')]");

        public GroupsMigrationForm() : base(TitleLocator, "Groups migration form")
        {
        }

        private readonly Button filter = new Button(By.XPath("//a[contains(text(),'Filter')]"), "Filter button");

        private readonly Button actionsDropdownButton =
            new Button(
                By.XPath(
                    "//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
                "Actions dropdown");

        private readonly Label security = new Label(By.XPath("//label[contains(text(),'Security')]"), "Security label");

        private readonly Label distribution = new Label(By.XPath("//label[contains(text(),'Distribution')]"), "Distribution label");

                
        public new void SyncUserByLocator(string locator)
        {
            WaitForAjaxLoad();
            ScrollToTop();
            Log.Info("Syncing group by locator: " + locator);
            WaitForAjaxLoad();
            SelectEntryBylocator(locator);
            SelectAction(ActionType.Sync);
            Apply();
        }

        public void ClickOnFilter()
        {
            filter.Click();
        }

        public void ClickSecurityRadio()
        {
            UnCheckAllFilters();
            security.Click();
        }

        public void ClickDistributionRadio()
        {
            UnCheckAllFilters();
            distribution.Click();
        }

        public void CheckSyncIsDisabledForGroups()
        {
            WaitForAjaxLoad();
            foreach (IWebElement group in Browser.GetDriver().FindElements(By.XPath("//td[child::div[@class='checkbox']]//input")))
            {
                group.Click();
                SelectAction(ActionType.Sync);
                CheckApplyButtonIsDisabled();
                group.Click();
            }
        }

        public void CheckSyncIsEnabledForGroups()
        {
            WaitForAjaxLoad();
            foreach (IWebElement group in Browser.GetDriver().FindElements(By.XPath("//td[child::div[@class='checkbox']]//input")))
            {
                group.Click();
                SelectAction(ActionType.Sync);
                CheckApplyButtonIsEnabled();
                group.Click();
            }
        }

        private void UnCheckAllFilters()
        {
            foreach (IWebElement filter in Browser.GetDriver().FindElements(By.XPath("//div[ancestor::div[@class='filter-list']]//input")))
            {
                if (Convert.ToBoolean(filter.GetAttribute("checked")))
                {
                    filter.Click();
                }
            }
        }

        public new void SelectAction(ActionType type)
        {
            OpenActionsDropdown();
            ChooseAction(type.GetValue());
        }

        public new void OpenActionsDropdown()
        {
            Log.Info("Opening Actions dropdown");
            ScrollToElement(actionsDropdownButton.GetElement());
            actionsDropdownButton.Click();
            try
            {
                expandedActionsDropdownButton.WaitForElementPresent(5000);
            }
            catch (Exception)
            {
                Log.Info("Actions dropdown is not ready");
                actionsDropdownButton.Click();
            }
        }
    }
}
