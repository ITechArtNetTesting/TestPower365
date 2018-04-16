using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
	public class DefineMigrationWavesForm : BaseWizardStepForm
	{        

        private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Would you like to define your migration waves now')]");

		public DefineMigrationWavesForm(Guid driverId) : base(TitleLocator, "Define migration waves form",driverId)
		{
            this.driverId = driverId;
            yesWavesLabel = new Label(By.XPath("//label[contains(@for, 'wavesYes')]"), "Yes, define waves now label",driverId);
            yesWavesRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'wavesYes')]"), "Yes, define waves now radiobutton",driverId);
            noWavesLabel = new Label(By.XPath("//label[contains(@for, 'wavesNo')]"), "No, thanks label",driverId);
            noWavesRadioButton = new RadioButton(By.XPath("//input[contains(@id, 'wavesNo')]"), "No, thanks radiobutton",driverId);
        }
		private Label yesWavesLabel ;
		private RadioButton yesWavesRadioButton ;
		private Label noWavesLabel ;
		private RadioButton noWavesRadioButton ;

		public void SelectYes()
		{
			Log.Info("Selecting yes");
			yesWavesLabel.Click();
			try
			{
				yesWavesRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				yesWavesLabel.Click();
			}
		}

		public void SelectNo()
		{
			Log.Info("Selecting no");
			noWavesLabel.Click();
			try
			{
				Thread.Sleep(3000);
				noWavesRadioButton.WaitForSelected(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				noWavesLabel.Click();
			}
		}
	}
}
