using System;
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
		protected BaseWizardStepForm(By TitleLocator, string name,Guid driverId) : base(TitleLocator, name,driverId)
		{
            this.driverId = driverId;
            nextButton =new Button(By.XPath("//div[contains(@class, 'wizard-footer')]//button[contains(@class, 'pull-right')][not(@disabled='')]"), "Next button",driverId);
            backButton = new Button(By.XPath("//button[contains(@data-bind, 'goBack')]"), "Back button",driverId);
            _titleLocator = TitleLocator;
		}
		protected Button nextButton ;
		protected readonly Button backButton ;
		public void GoNext()
		{
			Log.Info("Going next");
			nextButton.Click();
		    try
		    {
                new Label(_titleLocator, "Form locator",driverId).WaitForElementDisappear(30000);
		    }
		    catch (Exception)
		    {
		        nextButton = new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')][not(contains(@class, 'close'))]"), "Next button",driverId);
                nextButton.Click();
            }
		}

		public void GoBack()
		{
			Log.Info("Going back");
			backButton.Click();
		}
	}
}
