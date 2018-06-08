using BinaryTree.Power365.AutomationFramework.Dialogs;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    //@@@ Todo: Row locator that contains LowerCaseTextLocatorFormat is too broad for tables, can find elements anywhere on the page, should prepend //tr for table row
    public class TableElement : Element
    {
        private readonly string _rowInputAncestor = "/ancestor::tr//input";
        private readonly string _tabPaneLocator = "//div[contains(@class,'tab-pane active')]"; 
        private readonly string _rowTextAncestorFormat = "/ancestor::tr//*[contains(text(), '{0}')]";

        private By _nextPageLocator = By.XPath("//ul[contains(@class, 'pagination')]/li[contains(@class, 'active')]/following-sibling::li[1]/a");

        public TableElement(By locator, IWebDriver webDriver)
            : base(LogManager.GetLogger(typeof(TableElement)), locator, webDriver) { }

        public void ClickRowByValue(string value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowClassLocator = rowEntryLocator;

            //If it is part of a tab-pane there could be duplicates on the page
            if (IsElementVisible(By.XPath(_tabPaneLocator), 0))
                rowClassLocator = string.Format("{0}{1}", _tabPaneLocator, rowEntryLocator);
           
            var rowEntryInputAncestorLocator = string.Format("{0}{1}", rowClassLocator, _rowInputAncestor);
            var rowEntry = By.XPath(rowClassLocator);

            ClickElementBy(rowEntry);
            
            var rowCheckbox = By.XPath(rowEntryInputAncestorLocator);
            if (!IsElementSelectedState(rowCheckbox, true))
                throw new Exception("Failed to select row.");
        }

        public void ClickRowLinkByValue(string value, string linkLocator)
        {
            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowEntryLinkLocator = string.Format("{0}{1}", rowEntryLocator, linkLocator);
            var rowEntryLink = By.XPath(rowEntryLinkLocator);
            HoverElement(rowEntryLink, 0);
            ClickElementBy(rowEntryLink);
        }

        public T DoubleClickRowByValue<T>(string value)
            where T: ModalDialogBase
        {
            if (value == null)
                throw new ArgumentNullException("value");

            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, value.ToLowerInvariant());
            var rowClassLocator = rowEntryLocator;

            if (IsElementVisible(By.XPath(_tabPaneLocator), 0))
                rowClassLocator = string.Format("{0}{1}", _tabPaneLocator, rowEntryLocator);                  
            
            DoubleClickElementBy(By.XPath(rowClassLocator));
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

        public bool RowExists(string entry, int timeoutInSec = 5, int pollIntervalSec = 0)
        {
            var rowEntryLocator = string.Format(LowerCaseTextLocatorFormat, entry.ToLowerInvariant());
            var rowEntry = By.XPath(rowEntryLocator);
            return IsElementExists(rowEntry, timeoutInSec, pollIntervalSec);
        }

        public bool PageToRow(string entry)
        {
            if (RowExists(entry, 0))
                return true;

            HoverElement(_nextPageLocator);
            var nextPageElement = FindExistingElement(_nextPageLocator);

            if (nextPageElement == null)
                return false;

            do
            {
                nextPageElement.Click();

                if (RowExists(entry, 0))
                    return true;
            }
            while ((nextPageElement = FindExistingElement(_nextPageLocator)) != null);

            return false;
        }
    }
}
