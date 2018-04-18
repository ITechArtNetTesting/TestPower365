using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class UploadDistributionListForm : UploadFilesForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Upload your list of distribution groups')]");

		public UploadDistributionListForm() : base(TitleLocator, "Upload distribution list form")
		{
		}


	}
}
