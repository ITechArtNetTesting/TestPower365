using OpenQA.Selenium;
using OpenQA.Selenium.Support.PageObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IWebElement EmailFromFile { get; set; }

        [FindsBy(How = How.Id, Using = "projectName")]
        IWebElement CallProjectInput { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/div[@class='wizard-footer']/button/span[text()='Back']")]
        IWebElement BackButton { get; set; }

        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='Next']")]
        IWebElement NextButton { get; set; }

        public void CallProjectWithKeys(string keys)
        {
            CallProjectInput.SendKeys(keys);
        }

        public void ChooseEmailFromFileProjectType()
        {                        
            Performe.Click(EmailFromFile);
        }

        public void ClickAddTenantButton()
        {
            Performe.Click(AddTenantButton,1);
        }

        public void ClickBackButton()
        {            
            Performe.Click(BackButton);
        }

        public void ClickNextButton()
        {            
            Performe.Click(NextButton);
        }

        public void SendRandomKeysToDescription()
        {
            Performe.SendKeys(DescriptionInput, new string(Enumerable.Repeat("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 15).Select(s => s[new Random().Next(s.Length)]).ToArray()));
        }
    }
}
