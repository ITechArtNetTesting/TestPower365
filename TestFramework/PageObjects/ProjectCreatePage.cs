using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Framework;
using TestFramework.PageObjects.BasePages;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Waiters;

namespace TestFramework.PageObjects
{
    public class ProjectCreatePage :BasePage, IProjectCreatePage
    {
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='AddTenant']")]
        IWebElement AddTenantButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/input[@id='projectDescription']")]
        IWebElement DescriptionInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/label[@for='mailOnlyRadio']/span[@data-translation='EmailFromFile']")]
        IWebElement EmailFromFileProjectType { get; set; }

        [FindsBy(How = How.Id, Using = "projectName")]
        IWebElement CallProjectInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/div[@class='wizard-footer']/button/span[text()='Back']")]
        IWebElement BackButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='Next']")]
        IWebElement NextButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/input[@type='file']")]
        IWebElement SelectFile { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='Submit']")]
        IWebElement SubmitButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='EmailWithDiscovery']")]
        IWebElement EmailWithDiscoveryProjectType { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/div[@data-bind='foreach: tenants']//*/label")]
        IList<IWebElement> TenantsToSelect { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/div[@data-bind='foreach: sourceDomains']//*/label")]
        IList<IWebElement> DomainsToSelect { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/label[@for='groupsRadio']")]
        IWebElement SelectUsersByActiveDirectoryGroup { get; set; }

        [FindsBy(How = How.Id, Using = "groupSelector")]
        IWebElement FindGroupInput { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/div[@class='dropdown dropdown-large open']//*/a")]
        IWebElement FoundGroup { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='NoThanks']")]
        IWebElement NoThanksToDefineMyMigrationWavesNow { get; set; }


        public void CallProjectWithKeys(string keys)
        {
            CallProjectInput.SendKeys(keys);
        }

        public void ChooseEmailFromFileProjectType()
        {                        
            Perform.Click(EmailFromFileProjectType);
        }

        public void ClickAddTenantButton()
        {
            Perform.Click(AddTenantButton);
        }

        public void ClickBackButton()
        {            
            Perform.Click(BackButton);
        }

        public void ClickNextButton()
        {            
            Perform.Click(NextButton);
        }

        public void UploadFile(string keys)
        {
            Perform.UploadFile(SelectFile, Path.GetFullPath(RunConfigurator.ResourcesPath + keys));
        }

        public void SendRandomKeysToDescription()
        {
            Perform.SendKeys(DescriptionInput, new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 15).Select(s => s[new Random().Next(s.Length)]).ToArray()));
        }

        public void ClickSubmitButton()
        {
            Perform.Click(SubmitButton);
        }

        public void ChooseEmailWithDiscoveryProjectType()
        {
            Perform.Click(EmailWithDiscoveryProjectType);
        }

        public void SelectTenantByName(string name)
        {
            foreach (var tenant in TenantsToSelect)
            {
                if (tenant.Text == name.ToUpper())
                {
                    Perform.Click(tenant);
                }
            }
        }

        public void SelectDomainByName(string name)
        {
            foreach (var domain in DomainsToSelect)
            {
                if (domain.Text == name.ToUpper())
                {
                    Perform.Click(domain);
                }
            }
        }

        public void ChooseSelectUsersByActiveDirectoryGroup()
        {
            Perform.Click(SelectUsersByActiveDirectoryGroup);
        }

        public void SendKeysToFindGroupInput(string groupName)
        {
            Perform.SendKeys(FindGroupInput, groupName);
        }

        public void ClickFoundGroup()
        {
            Perform.Click(FoundGroup);
        }

        public void SelectNoThanksToDefineMyMigrationWavesNow()
        {
            Perform.Click(NoThanksToDefineMyMigrationWavesNow);
        }
    }
}
