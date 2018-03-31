using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;

namespace TestFramework.PageObjects
{
    public class ProjectOverviewPage : BasePage, IProjectOverviewPage
    {        
        [FindsBy(How = How.XPath, Using = "//*/a[@data-bind='attr: { href: allGroupsLink }']/span")]
        IWebElement GroupsEditButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/a[@data-bind='attr: { href: discoveryLink }']")]
        IWebElement DiscoveryOverviewEditButton { get; set;}

        [FindsBy(How = How.XPath, Using = "//*[contains(text(), 'Public folders')]/ancestor::div[contains(@class, 'ibox-title')]//a")]
        IWebElement PublicFoldersEditButton { get; set; }

        public void ClickDiscoveryOverviewEditButton()
        {
            Perform.Click(DiscoveryOverviewEditButton);
        }

        public void ClickGroupsEditButton()
        {
            Perform.Click(GroupsEditButton);
        }

        public void ClickPublicFoldersEditButton()
        {
            Perform.Click(PublicFoldersEditButton);
        }
    }
}
