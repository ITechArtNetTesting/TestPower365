﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public abstract class BaseWizardStepForm : BaseForm
	{
	    private By _titleLocator;
		protected BaseWizardStepForm(By TitleLocator, string name) : base(TitleLocator, name)
		{
		    _titleLocator = TitleLocator;
		}
		protected Button nextButton =
			new Button(By.XPath("//div[contains(@class, 'wizard-footer')]//button[contains(@class, 'pull-right')][not(@disabled='')]"), "Next button");
		protected readonly Button backButton = new Button(By.XPath("//button[contains(@data-bind, 'goBack')]"), "Back button");
        private readonly Label _firstTenantOkStatus = new Label(By.XPath("//div[child::h6[@data-translation='Tenants']]//tr[1]//span[contains(text(),'OK')]"), "Ok status of the first tenant");
        private readonly Label _secondTenantOkStatus = new Label(By.XPath("//div[child::h6[@data-translation='Tenants']]//tr[2]//span[contains(text(),'OK')]"), "Ok status of the second tenant");

        public void GoNext()
		{
			Log.Info("Going next");
			nextButton.Click();
		    try
		    {
                new Label(_titleLocator, "Form locator").WaitForElementDisappear(30000);
		    }
		    catch (Exception)
		    {
		        nextButton = new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')][not(contains(@class, 'close'))]"), "Next button");
                nextButton.Click();
            }
		}

		public void GoBack()
		{
			Log.Info("Going back");
			backButton.Click();
		}

        public bool AllTenantsHasOkStatus()
        {
            _firstTenantOkStatus.WaitForElementIsVisible();
            _secondTenantOkStatus.WaitForElementIsVisible();
            return _firstTenantOkStatus.IsElementVisible() && _secondTenantOkStatus.IsElementVisible();
        }
    }
}
