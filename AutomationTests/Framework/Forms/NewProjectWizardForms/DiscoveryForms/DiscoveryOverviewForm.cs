using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using Product.Framework.Elements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
    public class DiscoveryOverviewForm:BaseForm
    {        
        private static readonly By TitleLocator = By.XPath("//*");

        Element tenants = new Element(By.XPath("//div[@id='discovery']//tbody//tr"), "List of tenants");


        public DiscoveryOverviewForm():base(TitleLocator,"Discovery overview")
        {            
        }

        public void VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(string clientName)
        {
            WaitForAjaxLoad();
            var clientId= queryExecuter.SelectClientIdByName(clientName);

            for (int i = 0; i < tenants.GetElements().Count; i++)
            {
                var sqlFrequency = queryExecuter.SelectDiscoveryFrequencyHours(clientId, tenants.GetElements()[i].FindElement(By.XPath(".//strong")).Text);
                var uiFrequency = Convert.ToInt32(tenants.GetElements()[i].FindElement(By.XPath(".//span[@data-bind='text: discoveryRunInterval']")).Text);
                Assert.AreEqual(sqlFrequency, uiFrequency);
            }
        }

        public void ChangeDiscoveryFrequencyHours(int number)
        {
            for (int i = 0; i < tenants.GetElements().Count; i++)
            {
                new Actions(Browser.GetDriver()).MoveToElement(tenants.GetElements()[i]).Build().Perform();
                tenants.GetElements()[i].FindElement(By.XPath(".//input")).Clear();
                tenants.GetElements()[i].FindElement(By.XPath(".//input")).SendKeys(Convert.ToString(number));
            }
            Browser.GetDriver().Navigate().Refresh();
        }
    }
}
