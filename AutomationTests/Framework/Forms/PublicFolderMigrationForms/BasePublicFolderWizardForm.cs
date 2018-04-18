using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.PublicFolderMigrationForms
{
	public class BasePublicFolderWizardForm : BaseForm
	{
		public BasePublicFolderWizardForm(By TitleLocator, string name) : base(TitleLocator, name)
		{
		}

		protected readonly Button nextButton =
			new Button(By.XPath("//button[contains(@class, 'pull-right')][not(@disabled='')]"), "Next button");
		public void GoNext()
		{
			Log.Info("Going next");
			nextButton.Click();
		}
	}
}
