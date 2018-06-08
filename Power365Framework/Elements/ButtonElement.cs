using OpenQA.Selenium;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Dialogs;
using log4net;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class ButtonElement : Element
    {
        public ButtonElement(By locator, IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(ButtonElement)), locator, webDriver) { }

        public void Click(int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            ClickElementBy(Locator, timeoutInSec, pollIntervalInSec);
        }

        public T Click<T>(int timeoutInSec = 5, int pollIntervalInSec = 0)
            where T: PageBase
        {
            return ClickElementBy<T>(Locator, timeoutInSec, pollIntervalInSec);
        }
        
        public T ClickModal<T>(int timeoutInSec = 5, int pollIntervalInSec = 0)
            where T : ModalDialogBase
        {
            return ClickModalElementBy<T>(Locator, timeoutInSec, pollIntervalInSec);
        }
    }
}