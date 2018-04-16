using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class WhichUsersShareCalendarForm : BaseWizardStepForm
	{        
        private readonly static By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'How should we identify the users that will share calendar')]");

		public WhichUsersShareCalendarForm(Guid driverId) : base(TitleLocator, "Which users would you like to share calendar form",driverId)
		{
            this.driverId = driverId;
            byActiveDirectoryLabel = new Label(By.XPath("//label[contains(@for, 'importGroupsRadio')]"), "By active directory group label",driverId);
            byActiveDirectoryRadioButton = new RadioButton(By.Id("importGroupsRadio"), "By active directory group radiobutton",driverId);
        }

		private Label byActiveDirectoryLabel;
		private RadioButton byActiveDirectoryRadioButton ;

		public void SelectByAd()
		{
			Log.Info("Selecting by AD group");
			byActiveDirectoryLabel.Click();
			try
			{
				byActiveDirectoryRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				byActiveDirectoryLabel.Click();
			}
		}
	}
}
