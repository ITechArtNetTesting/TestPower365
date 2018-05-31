using System;
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
        private readonly By _tenantsEditLink = By.XPath("//div[contains(@class, 'ibox-title')]//*[text()[contains(translate(., 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'),'tenant')]]//ancestor::div[contains(@class, 'ibox-title')]//a");
        private readonly By _completedUsers =By.XPath("//*[contains(text(), 'users')]/ancestor::div[(@class='ibox')]//*[contains(text(), 'All Users')]/ancestor::tr//a[contains(@data-bind, 'completedNumber')]");
        private readonly By _rollbackUsers = By.XPath("//*[contains(text(), 'users')]/ancestor::div[(@class='ibox')]//*[contains(text(), 'All Users')]/ancestor::tr//a[contains(@data-bind, 'rollbackNumber')]");
        private readonly By _errorsUsers = By.XPath("//*[contains(text(), 'users')]/ancestor::div[(@class='ibox')]//*[contains(text(), 'All Users')]/ancestor::tr//a[contains(@data-bind, 'errors')]");



        public ProjectDetailsPage(IWebDriver webDriver)
            : base(_locator, webDriver) { }

        public ManageUsersPage ClickUsersEdit()
        {
            return ClickElementBy<ManageUsersPage>(_usersEditLink);
        }

        public EditTenantsPage ClickTenantsEdit()
        {
            return ClickElementBy<EditTenantsPage>(_tenantsEditLink);
        }

        public ManagePublicFoldersPage ClickPublicFoldersEdit()
        {
            return ClickElementBy<ManagePublicFoldersPage>(_publicFoldersEditLink);
        }

        public int GetRollbackUsersNumber()
        {
           var rollbackUsers = FindVisibleElement(_rollbackUsers);
           return int.Parse(rollbackUsers.Text);
        }

        public int GetErrorUsersNumber()
        {
            var errorUsers = FindVisibleElement(_errorsUsers);
            return int.Parse(errorUsers.Text);
        } 

        public int GetCompletedNumber()
        {
           var completedUsers = FindVisibleElement(_completedUsers);
           return int.Parse( completedUsers.Text);
        }

        public ManageUsersPage clickCompletedCountUsers()
        {
           return ClickElementBy<ManageUsersPage>(_completedUsers);
            
        }
    }
}
