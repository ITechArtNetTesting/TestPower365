using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using System;

namespace Product.Tests.CommonTests.Migration_Tests
{
    [TestClass]
    public class Verify_jobs_can_be_archived_TC23090 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        public void UIVerifyJobsCanBeArchivedForIntegration()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project2");
            string entry = RunConfigurator.GetSourceMailbox("client2", "project2", "entry20");
            UIVerifyJobsCanBeArchived(login, password, client, projectName, entry);
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void UIVerifyJobsCanBeArchivedForDiscovery()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project1");
            string entry = RunConfigurator.GetSourceMailbox("client2", "project1", "entry8");
            UIVerifyJobsCanBeArchived(login, password, client, projectName, entry);
        }

        public void UIVerifyJobsCanBeArchived(string login, string password, string client, string projectName, string entry)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(entry);
            User.AtUsersForm().PerfomActionForUser(entry, ActionType.Archive);
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().VerifyLineNotExist(entry);
        }
    }
}
