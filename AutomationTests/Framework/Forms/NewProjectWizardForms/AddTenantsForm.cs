using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class AddTenantsForm : BaseWizardStepForm
	{
        private Guid driverId;

        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'First, we have to add your tenants')]");

		private readonly Button addTenantButton = new Button(By.XPath("//span[contains(text(), 'Add tenant')]"),
			"Add tenant button");
		private readonly Label tenantLabel = new Label(By.XPath("//span[contains(@data-bind, 'tenantName')]"), "Tenant label");
		private readonly Button yesButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(text(), 'Yes')]"), "Yes button");
		private readonly Button noButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(text(), 'No')]"), "No button");
		public AddTenantsForm(Guid driverId) : base(TitleLocator, "Add tenants form")
		{
            this.driverId = driverId;
		}

		public void OpenOffice365LoginFormPopup()
		{
			Log.Info("Switching to new window");
            //Store.MainHandle = Browser.GetDriver().CurrentWindowHandle;
            Store.MainHandle = Driver.GetDriver(driverId).CurrentWindowHandle;
            //var finder = new PopupWindowFinder(Browser.GetDriver());
            var finder = new PopupWindowFinder(Driver.GetDriver(driverId));
            addTenantButton.WaitForElementPresent();
			addTenantButton.WaitForElementIsClickable();
			try
			{
				var popupWindowHandle =
					finder.Click(addTenantButton.GetElement());
                //Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
                Driver.GetDriver(driverId).SwitchTo().Window(popupWindowHandle);
            }
			catch (Exception)
			{
				Log.Info("Add tenant button is not ready");
				var popupWindowHandle =
					finder.Click(addTenantButton.GetElement());
                //Browser.GetDriver().SwitchTo().Window(popupWindowHandle);
                Driver.GetDriver(driverId).SwitchTo().Window(popupWindowHandle);
            }
		}

		public void WaitForTenantAdded(int count)
		{
			Log.Info("Waiting till tenants added: " + count);
			tenantLabel.WaitForSeveralElementsPresent(count);
		}
		public void RemoveTenant(string tenant)
		{
			Log.Info("Removing tenant");
			var removeTenantButton =
				new Button(By.XPath($"//span[contains(text(), '{tenant}')]/..//button[contains(@data-bind, 'removeTenant')]"),
					tenant + " remove button");
			removeTenantButton.Click();
			try
			{
				noButton.WaitForElementPresent(3000);
				noButton.Click();
			}
			catch (Exception)
			{
				Log.Info("No public folders attached to tenant");
			}
		}
	}
}