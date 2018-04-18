using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Product.Framework.Elements;

namespace Product.Framework.Forms.NewProjectWizardForms
{
	public class AddTenantsForm : BaseWizardStepForm
	{        
        private static readonly By TitleLocator = By.XPath("//div[contains(@class, 'wizard-body')]//*[contains(text(), 'First, we have to add your tenants')]");

        Store store;
		private readonly Button addTenantButton ;
		private readonly Label tenantLabel ;
		private readonly Button yesButton ;
		private readonly Button noButton ;
		public AddTenantsForm(Guid driverId,Store store) : base(TitleLocator, "Add tenants form",driverId)
		{
            this.store = store;
            this.driverId = driverId;
            addTenantButton = new Button(By.XPath("//span[contains(text(), 'Add tenant')]"),"Add tenant button",driverId);
            tenantLabel = new Label(By.XPath("//span[contains(@data-bind, 'tenantName')]"), "Tenant label",driverId);
            yesButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(text(), 'Yes')]"), "Yes button",driverId);
            noButton = new Button(By.XPath("//div[contains(@class, 'modal in')]//button[contains(text(), 'No')]"), "No button",driverId);
        }

		public void OpenOffice365LoginFormPopup()
		{
			Log.Info("Switching to new window");
            //Store.MainHandle = Browser.GetDriver().CurrentWindowHandle;
            store.MainHandle = Driver.GetDriver(driverId).CurrentWindowHandle;
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
					tenant + " remove button",driverId);
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