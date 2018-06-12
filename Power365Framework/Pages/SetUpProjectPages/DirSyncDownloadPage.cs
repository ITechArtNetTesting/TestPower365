using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree.Power365.AutomationFramework.Utilities;
using log4net;
using OpenQA.Selenium;

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

        public bool IsVidioElementPresent()
        {
            FiddlerProxy.StartWritingRequests();
            IsElementVisible(_video);
            ClickElementBy(_video);
            FiddlerProxy.StopFiddlerProxy();
            return IsElementVisible(_modalVideoDialog)&IsElementVisible(_video); 
        }
    }
}
