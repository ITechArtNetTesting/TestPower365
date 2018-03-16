using System;
using OpenQA.Selenium;
using Product.Framework.Elements;

namespace Product.Framework.Forms
{
	public class SubmittedProjectForm : BaseForm
	{
		private static readonly By TitleLocator = By.XPath("//h2[contains(text(), 'Your project has been submitted!')]");

		private readonly Label discoveryWarningLabel =
			new Label(
				By.XPath(
					"//h3[contains(text(), 'Please note that the initial discovery of users will take some time to complete.') and contains(text(), 'In the mean time, you may wish to:')]"),
				"Discovery warning label");

		private readonly Button reviewStatusButton =
			new Button(By.XPath("//div[contains(@id, 'submitted')]//a[contains(@href, 'projectId')]"),
				"Review your project status button");

		public SubmittedProjectForm() : base(TitleLocator, "Submitted project form")
		{
		}

		public void ReviewProject()
		{
			Log.Info("Opening project");
			reviewStatusButton.Click();
			try
			{
				reviewStatusButton.WaitForElementDisappear(10000);
			}
			catch (Exception)
			{
				Log.Info("Review project link is not ready");
				reviewStatusButton.Click();
			}
		}

		public void AssertDiscoveryWarningAppear()
		{
			Log.Info("Asserting discovery warning exists");
			discoveryWarningLabel.WaitForElementPresent();
		}
	}
}