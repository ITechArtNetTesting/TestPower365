using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
    [TestClass]
    public class TC23091_UI_Verify_submitting_delta_sync_job_for_a_Synced_user : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifySubmittingDeltaSyncJobForASyncedUser()
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
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().SyncUserByLocator(sourceMailbox3);
                User.AtUsersForm().Confirm();
                User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox3);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox3);
                User.AtUsersForm().VerifyStateIS("Syncing");
                User.AtUsersForm().WaitForJobIsCreated();
                User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
                User.AtUsersForm().WaitForSyncedState();           
                User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();
                User.AtUsersForm().CloseUserDetails();
                User.AtUsersForm().SyncUserByLocator(sourceMailbox3);
                User.AtUsersForm().Confirm();
                User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox3);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox3);
                User.AtUsersForm().VerifyStateIS("Syncing");
                User.AtUsersForm().WaitForJobIsCreated();
                User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
                User.AtUsersForm().WaitForSyncedState();             
                User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();                
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
