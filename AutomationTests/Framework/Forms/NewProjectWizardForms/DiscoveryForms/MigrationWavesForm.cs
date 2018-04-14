using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class MigrationWavesForm : BaseWizardStepForm
	{
        private Guid driverId;


        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Migration waves')]");

		public MigrationWavesForm(Guid driverId) : base(TitleLocator, "Migration waves form")
		{
            this.driverId = driverId;
		}
	}
}
