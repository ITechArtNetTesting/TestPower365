using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class BeginDiscoveryForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'begin discovery')]");

		public BeginDiscoveryForm(Guid driverId) : base(TitleLocator, "Let`s begin discovery form",driverId)
		{
            this.driverId = driverId;
		}

	}
}
