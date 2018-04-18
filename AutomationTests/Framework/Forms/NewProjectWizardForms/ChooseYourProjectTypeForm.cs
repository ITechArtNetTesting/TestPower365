using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class ChooseYourProjectTypeForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Choose your project type')]");

		private readonly Button mailOnlyButton = new Button(By.XPath("//label[contains(@for, 'mailOnlyRadio')]"),
			"Mail only button");

		private readonly Button mailWithDiscoveryButton =
			new Button(By.XPath("//label[contains(@for, 'mailWithDiscoveryRadio')]"), "Mail with discovery button");
		private readonly Button integrationButton = new Button(By.XPath("//label[contains(@for, 'integrationRadio')]"),
			"Integration button");
		public ChooseYourProjectTypeForm() : base(TitleLocator, "Choose your project type form")
		{
		}

		public void ChooseMailOnly()
		{
			Log.Info("Choosing Mail only project");
			mailOnlyButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				mailOnlyButton.Click();
			}
		}

		public void ChooseIntegration()
		{
			Log.Info("Choosing Integration project");
			integrationButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				integrationButton.Click();
			}
		}

		public void ChooseMailWithDiscovery()
		{
			Log.Info("Choosing Mail only project");
			mailWithDiscoveryButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				mailWithDiscoveryButton.Click();
			}
		}
	}
}