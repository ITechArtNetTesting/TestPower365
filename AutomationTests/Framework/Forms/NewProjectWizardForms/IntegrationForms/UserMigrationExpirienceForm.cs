using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class UserMigrationExpirienceForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'finish configuring your project')]");

		public UserMigrationExpirienceForm(Guid driverId) : base(TitleLocator, "Configure user migration expirience form")
		{
            this.driverId = driverId;
		}

	}
}
