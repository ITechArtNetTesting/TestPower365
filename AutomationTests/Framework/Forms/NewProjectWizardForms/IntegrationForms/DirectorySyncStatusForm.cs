using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class DirectorySyncStatusForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Below are your')]");

		public DirectorySyncStatusForm() : base(TitleLocator, "Durectory sync status")
		{
		}
	}
}
