using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class SelectTargetTenantForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Select A Target Tenant')]");
		public SelectTargetTenantForm() : base(TitleLocator, "Select target tenant form")
		{
		}
		public void SelectTenant(string tenant)
		{
			Log.Info("Selecting source tenant");
			var tenantButton = new Button(By.XPath($"//span[contains(text(), '{tenant}')]"), tenant + " button");
			tenantButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				tenantButton.Click();
			}
		}
	}
}