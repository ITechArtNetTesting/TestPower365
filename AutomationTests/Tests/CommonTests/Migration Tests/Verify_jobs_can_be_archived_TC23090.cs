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
            string user = RunConfigurator.GetSourceMailbox("client2", "project2", "entry17");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerformSearch(user);
                User.AtUsersForm().PerfomActionForUser(user, ActionType.Archive);
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().AssertUserIsNoLongerDisplayed(user);                
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void UIVerifyJobsCanBeArchivedForDiscovery()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project1");
            string user = RunConfigurator.GetSourceMailbox("client2", "project1", "entry8");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerformSearch(user);
                User.AtUsersForm().PerfomActionForUser(user, ActionType.Archive);
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().AssertUserIsNoLongerDisplayed(user);
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void UIVerifyJobsCanBeArchivedForMailOnly()
        {
            string login = RunConfigurator.GetUserLogin("client1");
            string password = RunConfigurator.GetPassword("client1");
            string client = RunConfigurator.GetClient("client1");
            string projectName = RunConfigurator.GetProjectName("client1", "project1");
            string user = RunConfigurator.GetSourceMailbox("client1", "project1", "entry11");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerformSearch(user);
                User.AtUsersForm().PerfomActionForUser(user, ActionType.Archive);
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().AssertUserIsNoLongerDisplayed(user);
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}
