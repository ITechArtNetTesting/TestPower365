using BinaryTree.Power365.AutomationFramework.Dialogs;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class TableElement : Element
    {
        private readonly string _rowInputAncestor = "/ancestor::tr//input";
        private readonly string _rowClassLocator = "//div[contains(@class,'tab-pane active')]"; 
        private readonly string _rowTextAncestorFormat = "/ancestor::tr//*[contains(text(), '{0}')]";
        private readonly string _logsLocator = "//ancestor::tr//*[contains(@class,'fa-download')]";

        public TableElement(By locator, IWebDriver webDriver)
            : base(locator, webDriver) { }

        public void ClickRowByValue(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowClassLocator = rowEntryLocator;

            if (IsElementVisible(By.XPath(_rowClassLocator))) //id we have class= 'tab-pane active'
            {
            rowClassLocator = string.Format("{0}{1}", _rowClassLocator, rowEntryLocator);
            }
           
            var rowEntryInputAncestorLocator = string.Format("{0}{1}", rowClassLocator, _rowInputAncestor);
            var rowEntry = By.XPath(rowClassLocator);

            ClickElementBy(rowEntry);
            
            var rowCheckbox = By.XPath(rowEntryInputAncestorLocator);
            if (!IsElementSelectedState(rowCheckbox, true))
                throw new Exception("Failed to select row.");
        }

        public T DoubleClickRowByValue<T>(string value)
            where T: ModalDialogBase
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowClassLocator = string.Format("{0}{1}", _rowClassLocator, rowEntryLocator);
            
            var rowEntry = By.XPath(rowClassLocator);
            DoubleClickElementBy(rowEntry);
            return (T)Activator.CreateInstance(typeof(T), WebDriver);
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

        public bool RowHasAnyValue(string entry, string[] values, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var bys = new List<By>();

            foreach (var value in values)
            {
                var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, entry.ToLowerInvariant());
                var rowTextAncestorLocator = string.Format(_rowTextAncestorFormat, value);
                var rowEntryTextLocator = string.Format("{0}{1}", rowEntryLocator, rowTextAncestorLocator);
                var rowEntryTextValue = By.XPath(rowEntryTextLocator);
                bys.Add(rowEntryTextValue);
            }
            
            return IsAnyElementExists(bys.ToArray(), timeoutInSec, pollIntervalSec);
        }

        public void ClickLogsByValue(string state)
        {
            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, state.ToLowerInvariant());
            var rowEntryLogsLocator = string.Format("{0}{1}", rowEntryLocator, _logsLocator);
            var rowEntryLog = By.XPath(rowEntryLogsLocator);
            ClickElementBy(rowEntryLog);
        }


    }
}
