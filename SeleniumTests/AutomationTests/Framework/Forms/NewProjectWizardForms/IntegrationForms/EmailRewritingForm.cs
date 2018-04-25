using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.PublicFolderMigrationForms;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class EmailRewritingForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(),'Email Rewriting')]");

		public EmailRewritingForm() : base(TitleLocator, "Email rewriting")
		{
		}

	}
}
