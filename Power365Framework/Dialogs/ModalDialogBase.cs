using BinaryTree.Power365.AutomationFramework.Elements;
using BinaryTree.Power365.AutomationFramework.Enums;
using log4net;
using OpenQA.Selenium;
using System;
namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
   public class ModalDialogBase : Element, IDisposable
      {
        private static By _locator = By.XPath("//*[contains(@class,'modal in')]//*[(contains(@class, 'close')) or (contains(text(), 'Cancel'))]"); //By.ClassName("close") - not unique
        private readonly By _closeButton = By.ClassName("close");

        private bool _isOpen = false;

        public ModalDialogBase(ILog logger, IWebDriver webDriver)
            : base(logger, _locator, webDriver)
        {
            _isOpen = true;
        }

        public void Close()
        {
            ClickElementBy(_closeButton);
            _isOpen = false;
        }

        public void Dispose()
        {
            if (_isOpen)
                Close();
        }
    }
}
