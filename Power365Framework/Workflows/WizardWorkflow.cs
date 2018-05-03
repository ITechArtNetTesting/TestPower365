using BinaryTree.Power365.AutomationFramework.Pages;
using OpenQA.Selenium;
namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public abstract class WizardWorkflow : WorkflowBase
    {
        private readonly By _backButton = By.ClassName("btn-back");
        private readonly By _nextButton = By.ClassName("btn-next");

        protected WizardWorkflow(PageBase rootPage, IWebDriver webDriver)
            : base(rootPage, webDriver) { }

        protected void ClickNext()
        {
            var nextButtonElement = FindClickableElement(_nextButton);
            nextButtonElement.Click();
        }

        protected void ClickBack()
        {
            var backButtonElement = FindClickableElement(_backButton);
            backButtonElement.Click();
        }
    }
}
