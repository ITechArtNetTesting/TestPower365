﻿using OpenQA.Selenium;
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
    public class ProjectsListPage : BasePage,IProjectsListPage
    {
        [FindsBy(How = How.XPath, Using = "//*/font[text()='NEW PROJECT']")]
        IWebElement NewProjectButton { get; set; }

        public void ClickNewProjectButton()
        {
            NewProjectButton.Click();
        }
    }
}