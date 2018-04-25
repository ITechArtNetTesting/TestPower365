using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class PublicFolderSourceFilePathForm : BasePublicFolderWizardForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'What is the path to the public folder that you want to migrate')]");

		public PublicFolderSourceFilePathForm() : base(TitleLocator, "What source file path form")
		{
		}

		private readonly TextBox filePathTextBox = new TextBox(By.XPath("//input[contains(@data-bind, 'textInput')]"), "File path textbox");

		public void SetFilePath(string path)
		{
			Log.Info("Setting file path to: "+path);
			filePathTextBox.ClearSetText(path);
		}
	}
}
