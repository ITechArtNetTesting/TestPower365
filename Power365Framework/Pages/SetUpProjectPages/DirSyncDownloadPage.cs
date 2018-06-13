using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace BinaryTree.Power365.AutomationFramework.Pages.SetUpProjectPages
{
    public class DirSyncDownloadPage : PageBase
    {
        private static readonly By _locator = By.XPath("//span[@data-translation='LetsDownloadPower365DirectorySyncLiteApplication']");
        public DirSyncDownloadPage(IWebDriver webDriver) : base(LogManager.GetLogger(typeof(DirSyncDownloadPage)), _locator, webDriver)
        {
        }
        private static readonly By _video = By.XPath("//img[@class='video-thumbnail']");
        private static readonly By _modalVideoDialog = By.Id("modalDialog");
        private readonly By _videoLoadingSpinner = By.XPath("//div[@class='video-spinner']");

        public bool IsVidioElementPresent()
        {
            IsElementVisible(_video);
            ClickElementBy(_video);            
            EvaluateElement(ExpectedConditions.InvisibilityOfElementLocated(_videoLoadingSpinner), 10, 1, delegate { });
            return IsElementVisible(_modalVideoDialog)&&IsElementVisible(_video); 
        }       
    }
}
