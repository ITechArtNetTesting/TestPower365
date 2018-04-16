using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFoldersScheduleForm : BasePublicFolderWizardForm
    {      

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to run this migration on a schedule')]");

		public PublicFoldersScheduleForm(Guid driverId) : base(TitleLocator, "Schedule sync form",driverId)
		{
            this.driverId = driverId;
            scheduleButton = new Button(By.XPath("//label[contains(@for, 'schedule')]"), "Schedule button",driverId);
            onDemandButton = new Button(By.XPath("//label[contains(@for, 'onDemand')]"), "On demand button",driverId);
        }
		private readonly Button scheduleButton ;
		private readonly Button onDemandButton ;

		public void SelectSchedule()
		{
			Log.Info("Schedule selecting");
			scheduleButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				scheduleButton.Click();
			}
		}
		public void SelectOnDemand()
		{
			Log.Info("On demand selecting");
			onDemandButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				onDemandButton.Click();
			}
		}
	}
}
