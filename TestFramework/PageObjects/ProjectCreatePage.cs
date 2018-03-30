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
            Performe.Click(EmailFromFileProjectType);
        }

        public void ClickAddTenantButton()
        {
            Performe.Click(AddTenantButton);
        }

        public void ClickBackButton()
        {            
            Performe.Click(BackButton);
        }

        public void ClickNextButton()
        {            
            Performe.Click(NextButton);
        }

        public void UploadFile(string keys)
        {
            Performe.UploadFile(SelectFile, Path.GetFullPath(RunConfigurator.ResourcesPath + keys));
        }

        public void SendRandomKeysToDescription()
        {
            Performe.SendKeys(DescriptionInput, new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 15).Select(s => s[new Random().Next(s.Length)]).ToArray()));
        }

        public void ClickSubmitButton()
        {
            Performe.Click(SubmitButton);
        }

        public void ChooseEmailWithDiscoveryProjectType()
        {
            Performe.Click(EmailWithDiscoveryProjectType);
        }

        public void SelectTenantByName(string name)
        {
            foreach (var tenant in TenantsToSelect)
            {
                if (tenant.Text == name.ToUpper())
                {
                    Performe.Click(tenant);
                }
            }
        }

        public void SelectDomainByName(string name)
        {
            foreach (var domain in DomainsToSelect)
            {
                if (domain.Text == name.ToUpper())
                {
                    Performe.Click(domain);
                }
            }
        }

        public void ChooseSelectUsersByActiveDirectoryGroup()
        {
            Performe.Click(SelectUsersByActiveDirectoryGroup);
        }

        public void SendKeysToFindGroupInput(string groupName)
        {
            Performe.SendKeys(FindGroupInput, groupName);
        }

        public void ClickFoundGroup()
        {
            Performe.Click(FoundGroup);
        }

        public void SelectNoThanksToDefineMyMigrationWavesNow()
        {
            Performe.Click(NoThanksToDefineMyMigrationWavesNow);
        }
    }
}
