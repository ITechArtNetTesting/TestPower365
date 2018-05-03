using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Workflows;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{   
    public class EditProjectPage : PageBase
    {
        private static By _locator = By.Id("editProjectContainer");
        
        public EditProjectPage(IWebDriver webDriver) 
            : base(_locator, webDriver) { }

    }
}