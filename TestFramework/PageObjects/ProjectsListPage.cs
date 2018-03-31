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
        
        [FindsBy(How = How.XPath, Using = "//*/span[@data-translation='Archive']")]
        IList<IWebElement> ArchiveButtons { get; set; }
        
        [FindsBy(How = How.XPath, Using = "//*/span[text()='Yes']")]
        IWebElement YesToArchiveButton { get; set; }

        public void ChooseProjectByName(string projectName)
        {
            foreach (var project in ListOfProjects)
            {
                if (project.Text == projectName.ToUpper())
                {
                    Perform.Click(project);
                    break;
                }
            }
        }

        public void ClickArchiveProjectByName(string projectName)
        {
            for (int i = 0; i < ListOfProjects.Count; i++)
            {
                if (ListOfProjects[i].Text == projectName.ToUpper())
                {
                    Perform.Click(ArchiveButtons[i]);
                    break;
                }
            }
        }

        public void ClickNewProjectButton()
        {                                    
            Perform.Click(NewProjectButton);
        }

        public void ClickYesToArchiveButton()
        {
            Perform.Click(YesToArchiveButton);
        }
    }
}
