using OpenQA.Selenium;
using BinaryTree.Power365.AutomationFramework.Pages;
using BinaryTree.Power365.AutomationFramework.Dialogs;

namespace BinaryTree.Power365.AutomationFramework.Elements
{
    public class ButtonElement : Element
    {
        public ButtonElement(By locator, IWebDriver webDriver) 
            : base(locator, webDriver) { }

        public T Click<T>(int timeoutInSec = 5, int pollIntervalInSec = 0)
            where T: PageBase
        {
            return ClickElementBy<T>(Locator, timeoutInSec, pollIntervalInSec);
        }

        public T ClickModal<T>(int timeoutInSec = 5, int pollIntervalInSec = 0)
            where T: ModalDialogBase
        {
            return ClickModelElementBy<T>(Locator, timeoutInSec, pollIntervalInSec);
        }

        public void Click(int timeoutInSec = 5, int pollIntervalInSec = 0)
        {
            ClickElementBy(Locator, timeoutInSec, pollIntervalInSec);
        }
    }
}