using OpenQA.Selenium;
using Product.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Product.Framework.Forms
{
    public class DiscoveryOverviewForm:BaseForm
    {
        public DiscoveryOverviewForm() : base(TitleLocator,"Discovery Overview Form")
        {
            integrationLabel.WaitForElementPresent();
        }
        protected Label integrationLabel => new Label(TitleLocator, "Integration");
        private static readonly By TitleLocator = By.XPath("//*[text()='Integration']");

        private readonly Element FirstTenant = new Element(By.XPath("//*/div[@data-bind='with: projectInfoViewModel']//*/div[@id='discovery']//*/tbody/tr[1]"), "FirstDiscoveryTenant");
        private readonly Element SecondTenant=new Element(By.XPath("//*/div[@data-bind='with: projectInfoViewModel']//*/div[@id='discovery']//*/tbody/tr[2]"), "SecondDiscoveryTenant");
        private readonly Button DisableFirstTenantButton = new Button(By.XPath("//*/div[@data-bind='with: projectInfoViewModel']//*/div[@id='discovery']//*/tbody/tr[1]//*/font[text()='Disable']"), "DisableFirstTenantButton");
        private readonly Button DisableSecondTenantButton = new Button(By.XPath("//*/div[@data-bind='with: projectInfoViewModel']//*/div[@id='discovery']//*/tbody/tr[2]//*/font[text()='Disable']"), "DisableSecondTenant");
        private readonly Button BackToDashboardButton = new Button(By.XPath("//*/span[@data-translation='BackToDashboard']"), "BackToDashboardButton");

        public void DisableTenants()
        {
            FirstTenant.Move();
            DisableFirstTenantButton.Click();
            SecondTenant.Move();
            DisableSecondTenantButton.Click();
        }
        public void BackToDashboard()
        {
            BackToDashboardButton.Click();
        }
    }
}
