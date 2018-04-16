using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class ConfigureEmailRewrittingForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to have email rewritten')]");

		public ConfigureEmailRewrittingForm(Guid driverId) : base(TitleLocator, "Do you want to configure email rewriting form",driverId)
		{
            this.driverId = driverId;
            yesLabel = new Label(By.XPath("//label[contains(@for, 'yesFolders')]"), "Yes label",driverId);
            yesRadioButton = new RadioButton(By.Id("yesFolders"), "Yes radiobutton",driverId);
            noLabel = new Label(By.XPath("//label[contains(@for, 'noFolders')]"), "No label",driverId);
            noRadioButton = new RadioButton(By.Id("noFolders"), "",driverId);
        }

		private readonly Label yesLabel ;
		private readonly RadioButton yesRadioButton ;
		private readonly Label noLabel ;
		private readonly RadioButton noRadioButton ;

		public void SelectYes()
		{
			Log.Info("Selecting yes");
			yesLabel.Click();
			try
			{
				yesRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				yesLabel.Click();
			}
		}

		public void SelectNo()
		{
			Log.Info("Selecting no");
			noLabel.Click();
			try
			{
				noRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				noLabel.Click();
			}
		}
	}
}
