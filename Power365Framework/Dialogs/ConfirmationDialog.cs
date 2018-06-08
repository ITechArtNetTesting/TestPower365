using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class ConfirmationDialog : ModalDialogBase
    {
        public ButtonElement YesButton
        {
            get
            {
                var yesButton = By.XPath(string.Format(_confirmationDialogButtonFormat, "Yes"));
                return new ButtonElement(yesButton, WebDriver);
            }
        }
        public ButtonElement NoButton
        {
            get
            {
                var noButton = By.XPath(string.Format(_confirmationDialogButtonFormat, "No"));
                return new ButtonElement(noButton, WebDriver);
            }
        }

        private readonly string _confirmationDialogButtonFormat = "//div[@id='confirmationDialog'][contains(@class, 'modal in')]//*[contains(text(), '{0}')]";

        public ConfirmationDialog(IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(ConfirmationDialog)), webDriver) { }

        public void Yes()
        {
            YesButton.Click();
        }

        public void No()
        {
            NoButton.Click();
        }
    }
}