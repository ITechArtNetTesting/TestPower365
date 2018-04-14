using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class ConfigureDirectorySyncForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'configure Directory Sync Pro')]");

		public ConfigureDirectorySyncForm(Guid driverId) : base(TitleLocator, "Configure directory sync")
		{
            this.driverId = driverId;
		}


	}
}
