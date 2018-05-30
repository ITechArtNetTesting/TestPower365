using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests
{
    [TestFixture]
    public class VerifyTheSelectActionOnTheManageErrorsPage_TC34736 : TestBase
    {
        public VerifyTheSelectActionOnTheManageErrorsPage_TC34736() : base() { }

        private ErrorsPage atErrorsPage;

        private void verifyTheSelectActionOnTheManageErrorsPage(string username, string password, string client, string projectName)
        {
            atErrorsPage = Automation.Common
                .SingIn(username, password)
                .ClientSelect(client)
                .ProjectSelect(projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();
            Assert.IsTrue(atErrorsPage.CheckExportAndDismissDisplayed(),"Export or Dismiss actions are not diplayed");
        }

        private void runTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);
            string _client = client.Name;
            string _username = client.Administrator.Username;
            string _password = client.Administrator.Password;
            string _projectName = project.Name;

            verifyTheSelectActionOnTheManageErrorsPage(_username, _password, _client, _projectName);
        }

        [Test]
        [Category("Integration")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_Integration_34736()
        {
            runTest("client2", "project2");
        }

        [Test]
        [Category("MailWithDiscovery")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_MD_34736()
        {
            runTest("client2", "project1");
        }

        [Test]        
        [Category("MailOnly")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_MO_34736()
        {
            runTest("client1", "project1");
        }
    }
}
