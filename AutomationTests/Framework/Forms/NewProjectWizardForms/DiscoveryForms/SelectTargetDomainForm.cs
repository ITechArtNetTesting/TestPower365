﻿using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class SelectTargetDomainForm : BaseWizardStepForm
	{
        private Guid driverId;


        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Select A Target Domain')]");

		public SelectTargetDomainForm(Guid driverId) : base(TitleLocator, "Select target tenant form")
		{
            this.driverId = driverId;
		}
		public void SelectDomain(string domain)
		{
			Log.Info("Selecting target domain");
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