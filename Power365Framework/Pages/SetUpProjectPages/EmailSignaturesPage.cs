using BinaryTree.Power365.AutomationFramework.Elements;
using log4net;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.AutomationFramework.Pages.SetUpProjectPages
{
    public class EmailSignaturesPage : PageBase
    {
        private static readonly By _locator = By.XPath("//*/span[@data-translation='EmailSignatures']");
        public EmailSignaturesPage(IWebDriver webDriver) : base(LogManager.GetLogger(typeof(EmailSignaturesPage)), _locator, webDriver)
        {
        }
        private static readonly By _video = By.XPath("//img[@class='video-thumbnail']");
        private static readonly By _modalVideoDialog = By.Id("modalDialog");
        private static readonly By _videoSrc = By.XPath("//video/source/@src");

        public bool IsVidioElementPresent()
        {
            IsElementVisible(_video);
            ClickElementBy(_video);

            return IsElementVisible(_modalVideoDialog) & IsElementVisible(_video);
        }       

    }
}
