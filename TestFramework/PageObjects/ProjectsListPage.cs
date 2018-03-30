using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class ProjectsListPage : BasePage,IProjectsListPage
    {
        [FindsBy(How = How.XPath, Using = "//*/font[text()='NEW PROJECT']")]
        IWebElement NewProjectButton { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/tbody//*/span[@data-bind='text: projectName']")]
        IList<IWebElement> ListOfProjects { get; set; }

        public void ChooseProjectByName(string projectName)
        {
            foreach (var project in ListOfProjects)
            {
                if (project.Text == projectName.ToUpper())
                {
                    Performe.Click(project);
                }
            }
        }

        public void ClickNewProjectButton()
        {                                    
            Performe.Click(NewProjectButton);
        }       
    }
}
