using BinaryTree.Power365.AutomationFramework.Elements;
using OpenQA.Selenium;

namespace BinaryTree.Power365.AutomationFramework.Pages
{
    public class ProjectDetailsPage : PageBase
    {
        private static readonly By _locator = By.Id("projectContainer");

        //@@@ REQ:ID
        private readonly By _usersEditLink = By.XPath("//div[contains(@data-bind, 'overallStatusViewModel')]//a[contains(@data-bind, 'totalLink')]//span");
        private readonly By _publicFoldersEditLink = By.XPath("//*[contains(text(), 'Public folders')]/ancestor::div[contains(@class, 'ibox')]//div[contains(@class, 'ibox-content')]//a[contains(@data-bind, 'totalLink')]");

        public ProjectDetailsPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        public ManageUsersPage ClickUsersEdit()
        {
            return ClickElementBy<ManageUsersPage>(_usersEditLink);
        }

        public ManagePublicFoldersPage ClickPublicFoldersEdit()
        {
            return ClickElementBy<ManagePublicFoldersPage>(_publicFoldersEditLink);
        }
    }
}
