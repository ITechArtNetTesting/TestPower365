using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
    class DiscoveryOverviewForm:BaseForm
    {
        private static readonly By TitleLocator = By.XPath("//span[contains(text(),'Integration')]");

        IList<IWebElement> tenants = Browser.GetDriver().FindElements(By.XPath("//div[@id='discovery']//tbody//tr"));

        IList<IWebElement> tenantNames= Browser.GetDriver().FindElements(By.XPath("//div[@id='discovery']//tbody//tr"));

        IList<IWebElement> discoveryRunIntervals= Browser.GetDriver().FindElements(By.XPath("//span[@data-bind='text: discoveryRunInterval']"));

        public DiscoveryOverviewForm():base(TitleLocator,"Discovery overview")
        {
            if (tenants.Count != discoveryRunIntervals.Count)
            {
                throw new Exception();
            }
        }

        public void CheckTheCurrentDiscoveryFrequencyForTheTenants()
        {
            for (int i = 0; i < discoveryRunIntervals.Count; i++)
            {

            }
        }

    }
}
