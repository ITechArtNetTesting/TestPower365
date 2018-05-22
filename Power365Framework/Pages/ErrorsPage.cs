using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ErrorsPage: PageBase
    {
        private static readonly By _locator = By.XPath("//div[@id='users']//input[contains(@placeholder,'error')]");

        public ErrorsPage(IWebDriver webDriver) : base(_locator,webDriver) { }
    }
}
