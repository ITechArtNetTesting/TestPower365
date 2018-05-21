using BinaryTree.Power365.AutomationFramework.Pages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class HelpPage: PageBase
    {
        private static readonly By _locator = By.XPath("//h1[text()=' Help Center']");

        public HelpPage(IWebDriver webDriver)
            : base(_locator, webDriver)
        {           
        }

        public string GatDomain()
        {
            return WebDriver.Url;
        }
    }
}