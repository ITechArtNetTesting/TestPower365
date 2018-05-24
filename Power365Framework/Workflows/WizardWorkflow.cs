using System;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Workflows
{
    public abstract class WizardWorkflow : WorkflowBase
    {
        private readonly By _backButton = By.ClassName("btn-back");
        //private readonly By _nextButton = By.ClassName("btn-next");
        private readonly By _nextButton = By.XPath("//div[@class='modal-content']//*[contains(@class, 'btn-next')]");

        protected WizardWorkflow(By locator, IWebDriver webDriver)
            : base(locator, webDriver) { }
        
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

        protected void ValidateStepBy(By by, int timeoutInSeconds = 5, int pollIntervalSec = 0)
        {
            var validationElement = FindVisibleElement(by, timeoutInSeconds, pollIntervalSec);
        }
    }
}
