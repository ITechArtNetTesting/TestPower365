using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class HowToMatchGroupsForm : HowToMatchUsersForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How would you like to match source distribution groups')]");

		public HowToMatchGroupsForm() : base(TitleLocator, "How to match distribution groups form")
		{
		}
	}
}
