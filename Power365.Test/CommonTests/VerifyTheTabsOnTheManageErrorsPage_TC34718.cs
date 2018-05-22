using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.AutomationFramework.Pages;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryTree.Power365.Test.CommonTests
{
    [TestClass]
    public class VerifyTheTabsOnTheManageErrorsPage_TC34718: TestBase
    {
        public VerifyTheTabsOnTheManageErrorsPage_TC34718() : base(LogManager.GetLogger(typeof(VerifyTheTabsOnTheManageErrorsPage_TC34718))) { }

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

        private void VerifyTheTabsOnTheManageErrorsPage(string client, string project)
        {
            SetTestCaseParams(client,project);
            atErrorsPage = Automation.Common
                .SingIn(_username, _password)
                .ClientSelect(_client)
                .ProjectSelect(_projectName)
                .GetPage<ProjectDetailsPage>()
                .Menu
                .ClickErrors();                
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifyTheTabsOnTheManageErrorsPage_MO_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client1", "project1");
            Assert.IsTrue(atErrorsPage.UsersTenantsAreDisplayed());
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifyTheTabsOnTheManageErrorsPage_MD_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client2", "project1");
            Assert.IsTrue(atErrorsPage.UsersTenantsAreDisplayed());
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void VerifyTheTabsOnTheManageErrorsPage_Integration_34718()
        {
            VerifyTheTabsOnTheManageErrorsPage("client2", "project2");
            Assert.IsTrue(atErrorsPage.UsersGrpoupsTenantsAreDisplayed());
        }
    }
}
