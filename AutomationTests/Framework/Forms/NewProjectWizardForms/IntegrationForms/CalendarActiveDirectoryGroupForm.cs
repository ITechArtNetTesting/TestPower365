using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class CalendarActiveDirectoryGroupForm : SelectMigrationGroupForm
	{
        private Guid driverId;

        private readonly static By TitleLocator =
			By.XPath(
                "//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Active Directory Group would you like to use to share calendar availability')]");

		public CalendarActiveDirectoryGroupForm(Guid driverId)
			: base(TitleLocator, "Which Active Directory group would you like to use to share calendar form")
		{
            this.driverId = driverId;
		}

	}
}
