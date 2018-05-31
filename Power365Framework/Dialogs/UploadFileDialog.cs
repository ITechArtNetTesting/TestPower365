using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Dialogs
{
    public class UploadFileDialog : ModalDialogBase
    {
        public UploadFileDialog(IWebDriver webDriver) : base(webDriver)
        {
        }
        private readonly By _selectFile = By.XPath("//div[contains(@class, 'modal in')]//input[@type='file']");


        public void ChooseFile(string fullPath, string fileName)
        {
           InputElement selectFile = new InputElement(_selectFile,WebDriver);
            selectFile.SendKeys(Path.GetFullPath(fullPath + fileName));
        }

    }
   

}
