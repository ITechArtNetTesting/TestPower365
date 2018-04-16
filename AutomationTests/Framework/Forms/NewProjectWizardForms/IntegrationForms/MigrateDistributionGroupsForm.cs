using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class MigrateDistributionGroupsForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to discover distribution groups')]");

		public MigrateDistributionGroupsForm(Guid driverId) : base(TitleLocator, "Migrate distribution groups form",driverId)
		{
            this.driverId = driverId;
            allGroupsFoundButton = new Button(By.XPath("//label[contains(@for, 'migrateAllGroupsRadio')]"), "All groups found",driverId);
            uploadListButton = new Button(By.XPath("//label[contains(@for, 'uploadGroupsRadio')]"), "Upload list button",driverId);
            uploadListRadioButton = new RadioButton(By.Id("uploadGroupsRadio"), "Upload list radiobutton",driverId);
        }
		private readonly Button allGroupsFoundButton ;
		private readonly Button uploadListButton ;
		private readonly RadioButton uploadListRadioButton ;
		public void SelectUploadList()
		{
			Log.Info("Selecting upload list");
			uploadListButton.Click();
			try
			{
				uploadListRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				uploadListButton.Click();
			}
		}

		public void SelectAllGroupsFound()
		{
			Log.Info("Selecting all groups found");
			allGroupsFoundButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				allGroupsFoundButton.Click();
			}
		}
	}
}
