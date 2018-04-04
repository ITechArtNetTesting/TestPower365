using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
    [TestClass]
    public class TC23092_UI_Verify_an_in_progress_job_can_be_stopped : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifyAnInProgressJobCanBeStopped()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
            string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry5']/..//source");

            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().GetUsersCount();
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().SyncUserByLocator(sourceMailbox3);
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox3);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox3);
                User.AtUsersForm().VerifyStateIS("Syncing");
                User.AtUsersForm().WaitForJobIsCreated();
                User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
                User.AtUsersForm().StopSyncing();
                User.AtUsersForm().ConfirmStop();
                User.AtUsersForm().WaitForSyncedState();
                User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();
                User.AtUsersForm().AssertSyncingWasStoped();
                User.AtUsersForm().CloseUserDetails();
            }
            catch (Exception ex)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw ex;
            }
        }
    }
}
