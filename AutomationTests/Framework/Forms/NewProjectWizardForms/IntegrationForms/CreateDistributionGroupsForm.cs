using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class CreateDistributionGroupsForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to create distribution groups')]");

		public CreateDistributionGroupsForm() : base(TitleLocator, "Create distributian groups")
		{
		}

		private readonly Button yesCreateGroupsButton = new Button(By.XPath("//label[contains(@for, 'createGroups')]"), "Yes create groups button");
		private readonly Button noDontCreateGroupsButton = new Button(By.XPath("//label[contains(@for, 'dontCreateGroups')]"), "No, don`t create groups button");

		public void SelectCreateGroups()
		{
			Log.Info("Selecting Create groups");
			yesCreateGroupsButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				yesCreateGroupsButton.Click();
			}
		}

		public void SelectDontCreateGroups()
		{
			Log.Info("Selecting Don`t create groups");
			noDontCreateGroupsButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				noDontCreateGroupsButton.Click();
			}
		}
	}
}
