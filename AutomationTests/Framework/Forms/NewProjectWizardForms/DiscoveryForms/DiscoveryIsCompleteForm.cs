using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class DiscoveryIsCompleteForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Discovery is Complete')]");

		public DiscoveryIsCompleteForm() : base(TitleLocator, "Discovery is complete form")
		{
		}
	}
}
