using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class ChooseYourProjectTypeForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Choose your project type')]");

		private readonly Button mailOnlyButton ;
        
        private readonly Button mailWithDiscoveryButton ;

        private readonly Button integrationButton ;
        
		public ChooseYourProjectTypeForm(Guid driverId) : base(TitleLocator, "Choose your project type form",driverId)
		{
            this.driverId = driverId;
            mailOnlyButton = new Button(By.XPath("//label[contains(@for, 'mailOnlyRadio')]"), "Mail only button",driverId);
            mailWithDiscoveryButton =new Button(By.XPath("//label[contains(@for, 'mailWithDiscoveryRadio')]"), "Mail with discovery button",driverId);
            integrationButton = new Button(By.XPath("//label[contains(@for, 'integrationRadio')]"),"Integration button",driverId);
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