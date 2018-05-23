using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class TableElement : ElementBase
    {
        private readonly string _rowInputAncestor = "/ancestor::tr//input";
        private readonly string _rowClassLocator = "//div[contains(@class,'tab-pane active')]"; 
        private readonly string _rowTextAncestorFormat = "/ancestor::tr//*[contains(text(), '{0}')]";

        public TableElement(By locator, IWebDriver webDriver)
            : base(locator, webDriver) { }

        public void ClickRowByValue(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowClassLocator = string.Format("{0}{1}", _rowClassLocator, rowEntryLocator);
            var rowEntryInputAncestorLocator = string.Format("{0}{1}", rowClassLocator, _rowInputAncestor);

            var rowEntry = By.XPath(rowClassLocator);
            ClickElementBy(rowEntry);
            
            var rowCheckbox = By.XPath(rowEntryInputAncestorLocator);
            if (!IsElementSelectedState(rowCheckbox, true))
                throw new Exception("Failed to select row.");
        }

        public PageBase DoubleClickRowByValue(string value, PageBase page)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowEntryInputAncestorLocator = string.Format("{0}{1}", rowEntryLocator, _rowInputAncestor);

            var rowEntry = By.XPath(rowEntryLocator);
           return DoubleClickElementBy<PageBase>(rowEntry);            
        }

        public By GetRowLocatorByValue(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowEntryInputAncestorLocator = string.Format("{0}{1}", rowEntryLocator, _rowInputAncestor);

            var rowEntry = By.XPath(rowEntryLocator);
            return rowEntry;
        }


        public bool RowHasValue(string entry, string value, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, entry.ToLowerInvariant());
            var rowTextAncestorLocator = string.Format(_rowTextAncestorFormat, value);
            var rowEntryTextLocator = string.Format("{0}{1}", rowEntryLocator, rowTextAncestorLocator);

            var rowEntryTextValue = By.XPath(rowEntryTextLocator);
            return IsElementExists(rowEntryTextValue, timeoutInSec, pollIntervalSec);
        }
    }
}
