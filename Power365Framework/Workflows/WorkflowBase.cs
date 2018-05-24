using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
using System;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public abstract class WorkflowBase : ElementBase
    {
        protected WorkflowBase(By locator, IWebDriver webDriver)
            : base(locator, webDriver) { }

        public virtual T GetPage<T>() 
            where T : PageBase
        {
            return (T)Activator.CreateInstance(typeof(T), new[] { WebDriver });
        }
    }

}
