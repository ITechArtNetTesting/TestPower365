using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.Menu
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    class VerifyTheSelectActionOnTheManageErrorsPage_TC34736 : TestBase
    {
        private void VerifyTheSelectActionOnTheManageErrorsPage(string username, string password, string client, string projectName)
        {
            var atErrorsPage = Automation.Common
                .SingIn(username, password)
                .MigrateAndIntegrateSelect()
                .ClientSelect(client)
                .ProjectSelect(projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();
                       
            Assert.IsTrue(atErrorsPage.CheckDismissAndExportAreDisplayed(), "Dismiss or export action are not displayed");
        }

        private void RunTest(string clientName, string projectName)
        {
            var client = Automation.Settings.GetByReference<Client>(clientName);
            var project = client.GetByReference<Project>(projectName);

            VerifyTheSelectActionOnTheManageErrorsPage(client.Administrator.Username, client.Administrator.Password, client.Name, project.Name);
        }

        [Test]
        [Category("UI")]
        [Category("Integration")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_Integration_34736()
        {
            RunTest("client2", "project2");
        }

        [Test]
        [Category("UI")]
        [Category("MailWithDiscovery")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_MD_34736()
        {
            RunTest("client2", "project1");
        }

        [Test]
        [Category("UI")]
        [Category("MailOnly")]
        public void VerifyTheSelectActionOnTheManageErrorsPage_MO_34736()
        {
            RunTest("client1", "project1");
        }
    }
}
