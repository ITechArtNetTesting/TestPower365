using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Product.Framework.Elements;
using System;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class SelectMigrationGroupForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Which Active Directory')]");
      

		private readonly TextBox groupTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'searchQuery')]"),
			"Group textbox");

        private readonly Label _noMatchingLable = new Label(By.XPath("//span[@data-translation='NoMatchingGroupsWereFoundWithin']"), "No matching message");
		
		public SelectMigrationGroupForm() : base(TitleLocator, "Select migration group form")
		{
		}

		public SelectMigrationGroupForm(By _TitleLocator, string _name) : base(_TitleLocator, _name)
		{
		}
        public void SetWrongGroup()
        {
            Log.Info("Setting wrong group");
            groupTextBox.ClearSetText("wrong group");
        }       

        public void SetGroup(string group)
		{            
			Log.Info("Setting group: " + group);            
			groupTextBox.ClearSetText(group);
            groupTextBox.DoubleClick();
        }

		public void SelectGroup(string group)
		{
			Log.Info("Selecting group: " + group);
			var groupButton = new Button(By.XPath($"//a[contains(@data-bind, 'selectGroup')][contains(text(), '{group}')]"),
				group + " button");
			groupButton.Click();
		}
        public bool CheckNoMatchingLable()
        {
            WaitForAjaxLoad();
            return _noMatchingLable.IsElementVisible();
        }
	}
}