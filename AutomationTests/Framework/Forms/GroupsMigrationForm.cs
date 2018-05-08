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

        Label foundGroup = new Label(By.XPath("//tr[child::td[child::div[@class='checkbox']]]//span[text()]"), "First found group");

        TextBox Search = new TextBox(By.Id("searchGroups"), "Search groups textbox");

        Button submitSearch = new Button(By.XPath("//button[@type='submit']"), "Submit search button");

        private readonly Button filter = new Button(By.XPath("//a[contains(text(),'Filter')]"), "Filter button");

        private readonly Button actionsDropdownButton =
            new Button(
                By.XPath(
                    "//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
                "Actions dropdown");

        private readonly Label security = new Label(By.XPath("//label[contains(text(),'Security')]"), "Security label");

        private readonly Label distribution = new Label(By.XPath("//label[contains(text(),'Distribution')]"), "Distribution label");

        public GroupsMigrationForm() : base(TitleLocator, "Groups migration form")
        {
        }        
                
        public void SyncUserByLocator(string locator)
        {
            WaitForAjaxLoad();
            ScrollToTop();
            Log.Info("Syncing group by locator: " + locator);
            WaitForAjaxLoad();
            SelectEntryBylocator(locator);
            SelectAction(ActionType.Sync);
            Apply();
        }   

        public void SearchGroup(string groupName)
        {
            Search.ClearSetText(groupName);
            submitSearch.Click();
            WaitForAjaxLoad();
        }

        public void CheckSyncIsEnabledForGroup(string groupName)
        {
            WaitForAjaxLoad();
            foundGroup.Click();
            SelectGroupAction(ActionType.Sync);
            CheckApplyButtonIsEnabled();
            foundGroup.Click();
        }

        public void CheckSyncIsDisabledForGroup(string groupName)
        {
            WaitForAjaxLoad();
            foundGroup.Click();
            SelectGroupAction(ActionType.Sync);
            CheckApplyButtonIsDisabled();
            foundGroup.Click();

        }

        private void UnCheckAllFilters()
        {
            WaitForAjaxLoad();
            foreach (IWebElement filter in Browser.GetDriver().FindElements(By.XPath("//div[ancestor::div[@class='filter-list']]//input")))
            {
                if (Convert.ToBoolean(filter.GetAttribute("checked")))
                {
                    filter.Click();
                }
            }
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
