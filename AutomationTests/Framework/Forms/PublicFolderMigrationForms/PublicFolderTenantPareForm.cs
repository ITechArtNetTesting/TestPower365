using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderTenantPareForm : BasePublicFolderWizardForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Which tenant pair')]");

		public PublicFolderTenantPareForm() : base(TitleLocator, "Tenant pare form")
		{
		}

		private readonly Button selectTenantPareButton = new Button(By.XPath("//button[contains(@data-toggle, 'dropdown')]"), "Select tenant pare button");
		private readonly Button expandedDropButton = new Button(By.XPath("//button[contains(@data-toggle, 'dropdown')][contains(@aria-expanded, 'true')]"), "Expanded dropdown");
		public void OpenDropDown()
		{
			Log.Info("Opening dropdown");
			selectTenantPareButton.Click();
			try
			{
				expandedDropButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Dropdown is not expanded");
				selectTenantPareButton.Click();
			}
		}

		public void SelectPare(string tenant)
		{
			Log.Info("Selecting pare with: "+tenant);
			Button tenantPareButton = new Button(By.XPath($"//*[contains(text(), '{tenant}')]"), tenant+" tenant pare");
			tenantPareButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				tenantPareButton.Click();
			}
		}
	}
}
