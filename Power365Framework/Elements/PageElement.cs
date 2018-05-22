using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class PageElement : ElementBase
    {
        public PageElement(By locator, IWebDriver webDriver) 
            : base(locator, webDriver) { }

        public bool IsVisible(int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            return IsElementVisible(Locator, timeoutInSec, pollIntervalSec);
        }
    }
}
