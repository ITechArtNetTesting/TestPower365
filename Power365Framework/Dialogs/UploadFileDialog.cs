using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;
using System.IO;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class UploadFileDialog : ModalDialogBase
    {
        public UploadFileDialog(IWebDriver webDriver) 
            : base(LogManager.GetLogger(typeof(UploadFileDialog)), webDriver) { }

        private readonly By _selectFile = By.XPath("//div[contains(@class, 'modal in')]//input[@type='file']");

        public void ChooseFile(string fullPath, string fileName)
        {
           InputElement selectFile = new InputElement(_selectFile,WebDriver);
            selectFile.SendKeys(Path.GetFullPath(fullPath + fileName));
        }

    }
}
