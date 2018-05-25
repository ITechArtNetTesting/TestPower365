using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;

namespace BinaryTree.Power365.Test.CommonTests
{
    [TestFixture]
    public class VerifyTheTabsOnTheManageErrorsPage_TC34718: TestBase
    {
        public VerifyTheTabsOnTheManageErrorsPage_TC34718() : base() { }

        private string _client;
        private string _username;        
        private string _password;
        private string _projectName;

        private ErrorsPage atErrorsPage;        

        private void SetTestCaseParams(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var _project = client.GetByReference<Project>(projectName);
            _client = client.Name;
            _username = client.Administrator.Username;
            _password = client.Administrator.Password;
            _projectName = _project.Name;
        }

        private void VerifyTheTabsOnTheManageErrorsPage(string client, string project,bool isIntegration=false)
        {
            SetTestCaseParams(client,project);
            atErrorsPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();
            if (isIntegration)
            {
                Assert.IsTrue(atErrorsPage.TabIsVisible("Groups"));
            }
            Assert.IsTrue(atErrorsPage.TabIsVisible("Users"));
            Assert.IsTrue(atErrorsPage.TabIsVisible("Tenants"));

        }

        [Test]
        public void VerifyTheTabsOnTheManageErrorsPage_MO_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client1", "project1");            
        }

        [Test]
        public void VerifyTheTabsOnTheManageErrorsPage_MD_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client2", "project1");          
        }

        [Test]
        public void VerifyTheTabsOnTheManageErrorsPage_Integration_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client2", "project2",true);           
        }
    }
}
