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
        private readonly Button groupActionsDropdownButton = new Button(By.XPath("//div[@class='ibox m-t-lg']//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"), "Actions dropdown in groups edit page");
        private readonly Button actionsDropdownButton =
            new Button(
                By.XPath(
                    "//div[contains(@class, 'dropdown-default')]//button[contains(@class, 'dropdown-toggle')]"),
                "Actions dropdown");

        private readonly Label security = new Label(By.XPath("//label[contains(text(),'Security')]"), "Security label");

        private readonly Label distribution = new Label(By.XPath("//label[contains(text(),'Distribution')]"), "Distribution label");
        private readonly RadioButton groupItem = new RadioButton(By.XPath("//td[child::div[@class='checkbox']]//input"), "Group item");

        Element foundGroup = new Element(By.XPath("//tr[child::td[child::div[@class='checkbox']]]//span[text()]"), "First found group");

        TextBox Search = new TextBox(By.Id("searchGroups"), "Search groups textbox");

        Button submitSearch = new Button(By.XPath("//button[@type='submit']"), "Submit search button");


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

        public void SearchGroup(string groupName)
        {
            Search.ClearSetText(groupName);
            submitSearch.Click();
            WaitForAjaxLoad();
        }

        public void ClickOnFilter()
        {
            filter.Click();
        }

        public void CheckSyncForGroup(string groupName, bool isEnabled= true)
        {
            Log.Info("Checking sync is enabled for group");
            WaitForAjaxLoad();
            foundGroup.Click();
            SelectAction(ActionType.Sync);
            CheckApplyButton(isEnabled);
            foundGroup.Click();
        }



        public void CheckSyncIsEnabledForGroup(string groupName)
        {
            Log.Info("Checking sync is enabled for group");
            WaitForAjaxLoad();
            foundGroup.Click();
            SelectAction(ActionType.Sync);
            CheckApplyButtonIsEnabled();
            foundGroup.Click();
        }

        public void CheckSyncIsDisabledForGroup(string groupName)
        {
            Log.Info("Checking sync is disabled for group");
            WaitForAjaxLoad();
            foundGroup.Click();
            SelectAction(ActionType.Sync);
            CheckApplyButtonIsDisabled();
            foundGroup.Click();

        }
        public void SelectAction(ActionType type)
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

        public void SelectGroupBylocator(string locator)
        {
           Log.Info("Selecting checkbox for: " + locator);
     
            var entryCheckboxButton =
                new RadioButton(
                    By.XPath(
                        $"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]/ancestor::tr//input"),
                    locator + " entry checkbox");
            var entryLabel = new Button(By.XPath(
                    $"//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'{locator.ToLower()}')]]"),
                locator + " entry label");

               ScrollToElement(entryLabel.GetElement());                 
               entryLabel.Click();
               entryCheckboxButton.WaitForSelected(5000);
         
        }


    }




}
