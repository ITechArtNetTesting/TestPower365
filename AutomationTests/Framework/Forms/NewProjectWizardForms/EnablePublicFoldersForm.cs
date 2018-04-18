using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class EnablePublicFoldersForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to migrate public folders')]");

		public EnablePublicFoldersForm() : base(TitleLocator, "Enable public folders form")
		{
		}

		private readonly Button yesButton = new Button(By.XPath("//label[contains(@for, 'yesFolders')]"), "Yes button");
		private readonly Button noFoldersButton = new Button(By.XPath("//label[contains(@for, 'noFolders')]"), "No button");

		public void SetYes()
		{
			Log.Info("Choosing Yes");
			yesButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				yesButton.Click();
			}
		}

		public void SetNo()
		{
			Log.Info("Choosing No");
			noFoldersButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				noFoldersButton.Click();
			}
		}
	}
}
