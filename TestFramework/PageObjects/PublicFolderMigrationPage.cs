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
    public class PublicFolderMigrationPage : BasePage, IPublicFolderMigrationPage
    {
        [FindsBy(How = How.XPath, Using = "//*/div[@id='publicFolderContainer']//*/tbody/tr/td[2]")]
        IList<IWebElement> PublicFolders { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='SelectAction']")]
        IWebElement ActionsDropdown { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[text()='Archive']")]
        IWebElement ArchiveAction { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='ApplyAction']")]
        IWebElement ApplyActionButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='BackToDashboard']")]
        IWebElement BackToDashboardButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/button[@data-bind='click: confirm']")]
        IWebElement YesSureArchiveButton { get; set; }

        public void ClickApplyActionButton()
        {
            Perform.Click(ApplyActionButton);
        }

        public void SelectAllPublicFolders()
        {
            foreach (var folder in PublicFolders)
            {
                Perform.Click(folder);
            }
        }

        public void SelectArchiveAction()
        {            
            Perform.Click(ArchiveAction);
        }        

        public void ClickBackToDashboardButton()
        {
            Perform.Click(BackToDashboardButton);
        }

        public void ClickYesSureArchiveButton()
        {
            Perform.Click(YesSureArchiveButton);
        }

        public void ClickActionsDropdown()
        {
            Perform.Click(ActionsDropdown);
        }
    }
}
