using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class SelectSourceDomainForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Select A Source Domain')]");
		public SelectSourceDomainForm() : base(TitleLocator, "Select source domain")
		{
		}
		public void SelectDomain(string domain)
		{
			Log.Info("Selecting source domain");
			var domainButton = new Button(By.XPath($"//span[contains(text(), '{domain}')]"), domain + " button");
			domainButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				domainButton.Click();
			}
		}
	}
}