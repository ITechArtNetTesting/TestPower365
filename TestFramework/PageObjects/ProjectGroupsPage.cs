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
    public class ProjectGroupsPage : BasePage, IProjectGroupsPage
    {        
        [FindsBy(How = How.XPath, Using = "//*/tbody//*/td[position()=2]/span[@class='notranslate']")]
        IWebElement TargetGroup { get; set; }      
        
        [FindsBy(How=How.Id,Using = "searchGroups")]
        IWebElement SearchGroupsInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/button[@type='submit']")]
        IWebElement SearchButton { get; set; }

        public void ClickTargetGroup()
        {
            Perform.Click(TargetGroup);
        }

        public void SearchGroupByName(string name)
        {
            Perform.SendKeys(SearchGroupsInput, name);
            Perform.Click(SearchButton);            
        }
    }
}
