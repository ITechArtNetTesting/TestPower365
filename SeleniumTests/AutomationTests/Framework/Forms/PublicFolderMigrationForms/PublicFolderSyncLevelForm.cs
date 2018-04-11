using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderSyncLevelForm : BasePublicFolderWizardForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to synchronize only')]");

		public PublicFolderSyncLevelForm() : base(TitleLocator, "Sync level form")
		{
		}
		private readonly Button topLevelOnlyButton = new Button(By.XPath("//label[contains(@for, 'topLevelOnly')]"), "Top level only button");
		private readonly Button allSubfoldersButton = new Button(By.XPath("//label[contains(@for, 'allSubfolders')]"), "All subfolders button");

		public void SelectTopLEvelOnly()
		{
			Log.Info("Top level only selecting");
			topLevelOnlyButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				topLevelOnlyButton.Click();
			}
		}

		public void SelectAllSubFolders()
		{
			Log.Info("All subfolders selecting");
			allSubfoldersButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				allSubfoldersButton.Click();
			}
		}
	}
}
