using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.PageObjects;
using Product.Framework.Elements;
using Product.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Framework.Forms.NewProjectWizardForms.DiscoveryForms
{
    public class DiscoveryOverviewForm : BaseForm
    {
        private static readonly By TitleLocator = By.XPath("//*");

        Element tenants = new Element(By.XPath("//div[@id='discovery']//tbody//tr"), "List of tenants");
        public DiscoveryOverviewForm() : base(TitleLocator, "Discovery overview")
        {
        }

        public void VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(string clientName, string projectName)
        {
            SQLExecuter queryExecuter = new SQLExecuter();
            var clientId = queryExecuter.SelectClientIdByName(clientName);
            var tenantId = queryExecuter.SelectTenantIdByName(projectName);


            WaitForAjaxLoad();
            for (int i = 0; i < tenants.GetElements().Count; i++)
            {
                var sqlFrequency = queryExecuter.SelectDiscoveryFrequencyHours(clientId, tenants.GetElements()[i].FindElement(By.XPath(".//strong")).Text, tenantId);
                var uiFrequency = Convert.ToInt32(tenants.GetElements()[i].FindElement(By.XPath(".//span[@data-bind='text: discoveryRunInterval']")).Text);
                Assert.AreEqual(sqlFrequency, uiFrequency);
            }

            //foreach (var element in tenantElements)
            //{
            //    string tenantName = element.FindElement(By.XPath(".//strong")).Text;
            //    var sqlFrequency = queryExecuter.SelectDiscoveryFrequencyHours(clientId, tenantName, tenantId);
            //    var uiFrequency = Convert.ToInt32(element.FindElement(By.XPath($"//span[@data-bind='text: discoveryRunInterval' and ancestor::tr//*[translate(text(), 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz')='{tenantName.ToLower()}']]")).Text);
            //    Assert.AreEqual(sqlFrequency, uiFrequency,String.Format("Tenant {0}. DiscoveryFrequencyHours does not match with UI value ", tenantName));
            //}
        }

        public void ChangeDiscoveryFrequencyHours(int number)
        {
            var tenantElements = tenants.GetElements();
            foreach (var element in tenantElements)
            {
                var tenantInput = element.FindElement(By.XPath(".//input"));
               new Actions(Browser.GetDriver()).MoveToElement(element).Build().Perform();
               element.FindElement(By.XPath(".//input")).Clear();
               element.FindElement(By.XPath(".//input")).SendKeys(Convert.ToString(number));
            }
            Browser.GetDriver().Navigate().Refresh();
        }
    }
}
