using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class SyncScheduleForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'sync schedule')]");
        private Button back = new Button(By.XPath("//span[@data-translation='Back']"), "Back button");

        public SyncScheduleForm() : base(TitleLocator, "Sync schedule form")
		{
		}
        public void ClickBackButton()
        {
            back.Click();
        }
    }
}
