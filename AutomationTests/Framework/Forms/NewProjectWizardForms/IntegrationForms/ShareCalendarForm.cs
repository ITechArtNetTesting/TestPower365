using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class ShareCalendarForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator =
			By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Would you like to share calendar availability')]");

		public ShareCalendarForm() : base(TitleLocator, "Share calendar form")
		{
		}

		private readonly Label yesLabel = new Label(By.XPath("//label[contains(@for, 'fbYes')]"), "Yes label");
		private readonly RadioButton yesRadioButton = new RadioButton(By.Id("fbYes"), "Yes radio");
		private readonly Label noLabel = new Label(By.XPath("//label[contains(@for, 'fbNo')]"), "No label");
		private readonly RadioButton noRadioButton = new RadioButton(By.Id("fbNo"), "No radio");

		public void SelectYes()
		{
			Log.Info("Selecting Yes");
			yesLabel.Click();
			try
			{
                Thread.Sleep(2000);
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
			Log.Info("Selecting No");
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
