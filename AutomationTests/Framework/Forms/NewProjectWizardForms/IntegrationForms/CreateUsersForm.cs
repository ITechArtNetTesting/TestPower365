using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms.IntegrationForms
{
	public class CreateUsersForm : BaseWizardStepForm
	{
		private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'Do you want to create users')]");

		public CreateUsersForm() : base(TitleLocator, "Create users form")
		{
		}

		public CreateUsersForm(By _TitleLocator) : base(_TitleLocator, "Create distribution groups form")
		{
		}

		private readonly Button createUsersButton = new Button(By.XPath("//label[contains(@for, 'createUsers')]"), "Create users button");
		private readonly Button dontCreateUsersButton = new Button(By.XPath("//label[contains(@for, 'dontCreateUsers')]"), "Dont create users button");

		public void SelectCreateUsers()
		{
			Log.Info("Selecting create users");
			createUsersButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				createUsersButton.Click();
			}
		}

		public void SelectDontCreate()
		{
			Log.Info("Selecting all groups found");
			dontCreateUsersButton.Click();
			try
			{
				nextButton.WaitForElementPresent(5000);
			}
			catch (Exception)
			{
				Log.Info("Radiobutton is not ready");
				dontCreateUsersButton.Click();
			}
		}
	}
}

