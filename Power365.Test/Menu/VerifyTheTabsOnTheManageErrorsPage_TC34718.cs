using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.Menu
{
    [TestFixture]
    public class VerifyTheTabsOnTheManageErrorsPage_TC34718: TestBase
    {
        public VerifyTheTabsOnTheManageErrorsPage_TC34718() : base() { }

              
        private void TestRun(string clientName, string projectName, bool isIntegration = false)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = _project.Name;
            VerifyTheTabsOnTheManageErrorsPage(_username, _password, _client, _projectName, isIntegration);
        }

        private void VerifyTheTabsOnTheManageErrorsPage(string _username, string _password , string _client, string  _projectName, bool isIntegration )
        {
           var errorsPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();
            if (isIntegration)
            {
                Assert.IsTrue(errorsPage.GroupsTab.IsVisible());
            }
            Assert.IsTrue(errorsPage.UsersTab.IsVisible());
            Assert.IsTrue(errorsPage.TenantsTab.IsVisible());

        }

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        public void VerifyTheTabsOnTheManageErrorsPage_MO_34718()
        {
            TestRun("client1", "project1");            
        }

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void VerifyTheTabsOnTheManageErrorsPage_MD_34718()
        {
            TestRun("client2", "project1");          
        }

        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void VerifyTheTabsOnTheManageErrorsPage_Integration_34718()
        {
            TestRun("client2", "project2",true);           
        }
    }
}
