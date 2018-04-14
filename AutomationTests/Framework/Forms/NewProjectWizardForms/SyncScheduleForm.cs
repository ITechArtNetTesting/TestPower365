using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SyncScheduleForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'sync schedule')]");

		public SyncScheduleForm(Guid driverId) : base(TitleLocator, "Sync schedule form")
		{
            this.driverId = driverId;
		}
	}
}
