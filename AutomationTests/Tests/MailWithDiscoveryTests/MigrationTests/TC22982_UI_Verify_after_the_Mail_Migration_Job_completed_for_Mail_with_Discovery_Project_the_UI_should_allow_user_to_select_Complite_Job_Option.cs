using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;

namespace Product.Tests.MailWithDiscoveryTests.MigrationTests
{
    [TestClass]
    public class TC22982_UI_Verify_after_the_Mail_Migration_Job_completed_for_Mail_with_Discovery_Project_the_UI_should_allow_user_to_select_Complite_Job_Option : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Migration")]
        public void VerifyAfterTheMailMigrationJobCompletedForMailWithDiscoveryProjectTheUIShouldAllowUserToSelectCompliteJobOption()
        {
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//name");
            string sourceMailbox3 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project1']/..//metaname[text()='entry1']/..//source");

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
                User.AtUsersForm().DownloadLogs();
                RunConfigurator.CheckLogsFileIsDownloaded();
                User.AtUsersForm().AssertDetailsSyncButtonIsEnabled();
                User.AtUsersForm().CompleteSync();
                User.AtUsersForm().ConfirmComplete();
                User.AtUsersForm().VerifyStateIS("Complete");
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
