using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderTargetFilePathForm : BasePublicFolderWizardForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//span[contains(text(), 'What target folder path do you want to migrate to')]");

		public PublicFolderTargetFilePathForm() : base(TitleLocator, "Target file path form")
		{
		}
		private readonly TextBox filePathTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput')]"), "File path textbox");

		public void SetFilePath(string path)
		{
			Log.Info("Setting file path to: " + path);
			filePathTextBox.ClearSetText(path);
		}
	}
}
