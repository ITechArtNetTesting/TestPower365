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

		public PublicFoldersScheduleForm() : base(TitleLocator, "Schedule sync form")
		{
		}
		private readonly Button scheduleButton = new Button(By.XPath("//label[contains(@for, 'schedule')]"), "Schedule button");
		private readonly Button onDemandButton = new Button(By.XPath("//label[contains(@for, 'onDemand')]"), "On demand button");

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
