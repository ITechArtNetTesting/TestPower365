using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.Migration_Tests
{
    [TestClass]
    public class VerifySubmittingDeltaSyncJobForASyncedUser_TC23091:LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserForMailOnly()
        {
            string login = RunConfigurator.GetUserLogin("client1");
            string password = RunConfigurator.GetPassword("client1");
            string client = RunConfigurator.GetClient("client1");
            string projectName = RunConfigurator.GetProjectName("client1", "project1");
            string entry = RunConfigurator.GetSourceMailbox("client2", "project2", "entry20");
            VerifySubmittingDeltaSyncJobForASyncedUser(login, password, client, projectName, entry);
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserForMailWithDiscovery()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project1");
            string entry = RunConfigurator.GetSourceMailbox("client2", "project2", "entry20");
            VerifySubmittingDeltaSyncJobForASyncedUser(login, password, client, projectName, entry);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void VerifySubmittingDeltaSyncJobForASyncedUserForIntegration()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project2");
            string entry = RunConfigurator.GetSourceMailbox("client2", "project2", "entry20");
            VerifySubmittingDeltaSyncJobForASyncedUser(login,password,client,projectName,entry);
        }

        private void VerifySubmittingDeltaSyncJobForASyncedUser(string login, string password, string client, string projectName, string entry)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(entry);
            User.AtUsersForm().OpenDetailsByLocator(entry);
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(entry, State.Syncing, 2700000, 10);
            User.AtUsersForm().WaitForState_DetailPage(entry, State.Synced, 2700000, 10);
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(entry, State.Syncing, 2700000, 10);
            User.AtUsersForm().WaitForState_DetailPage(entry, State.Synced, 2700000, 10);
            User.AtUsersForm().WaitForState_DetailPage(entry, State.Complete, 2700000, 10);
            User.AtUsersForm().CloseUserDetails();
        }
    }
}
