using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailWithDiscoveryTests.MigrationTests
{
    [TestClass]
    public class TC23090_UI_Verify_jobs_can_be_archived : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        public void UIVerifyJobsCanBeArchived()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project2");
            string user = RunConfigurator.GetSourceMailbox("client2", "project2", "entry2");
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
