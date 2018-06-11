using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class GroupsPage : ActionPageBase
    {
        private static readonly By _locator = By.XPath("//span[@data-translation='ViewSelectedGroups']");
        public GroupsPage(IWebDriver webDriver) : base(LogManager.GetLogger(typeof(GroupsPage)), _locator, webDriver)
        {
            visibleGroups = new List<String>();
            WaitForLoadComplete();
            foreach (var group in webDriver.FindElements(allGroups))
            {
                visibleGroups.Add(group.Text);
            }

        }

        private IList<String> visibleGroups;

        private readonly By SourceSortButton = By.XPath("//span[@data-translation='Source']");        

        private readonly By viewSelectedGroups = By.XPath("//span[@data-translation='ViewSelectedGroups']");

        private readonly By viewAllGroups = By.XPath("//*[text()='View all groups']");

        private readonly By allGroups = By.XPath("//tbody//td[2]/span");

        private readonly By allGroupsStatuses = By.XPath("//tbody//td[4]/span");

        private readonly By allSelectedGroups = By.XPath("//div[@class='ibox-content']//tbody//td[1]/span");

        private readonly By filter = By.XPath("//a[@data-translation='Filter']");

        private readonly string filters = "//div[@class='filter-list']//*[text()='{0}']";

        public void SortSource()
        {
            ClickElementBy(SourceSortButton);
        }

        public void SelectVisibleGroups()
        {
            foreach (var group in WebDriver.FindElements(allGroups))
            {
                group.Click();
            }            
        }

        public void ClickViewSelectedGroups()
        {
            ClickElementBy(viewSelectedGroups);
        }

        public void ClickViewAllGroups()
        {
            ClickElementBy(viewAllGroups);
        }

        public void ChooseFilter(string filter)
        {
            var _filter = By.XPath(string.Format(filters, filter));
            ClickElementBy(_filter);
        }

        public void ClickFilter()
        {
            ClickElementBy(filter);
            WaitForLoadComplete();
        }

        public bool CheckSortWorkingCorrectly()
        {
            bool result = true;
            WaitForLoadComplete();            
            var sortedGroups = visibleGroups.Reverse().ToList();
            var gottenGroups = WebDriver.FindElements(allSelectedGroups);
            if (gottenGroups.Count == sortedGroups.Count)
            {
                for (int i = 0; i < sortedGroups.Count; i++)
                {
                    if (sortedGroups[i] != gottenGroups[i].Text)
                    {
                        result = false;
                        break;
                    }
                }
            }
            else { result = false; }
            return result;
        }

        public bool CheckAllGroupsStatus(string status)
        {
            WaitForLoadComplete();
            bool result = true;
            foreach (var _status in WebDriver.FindElements(allGroupsStatuses))
            {
                if (_status.Text != status) { result = false;break; }
            }
            return result;
        }
    }
}
