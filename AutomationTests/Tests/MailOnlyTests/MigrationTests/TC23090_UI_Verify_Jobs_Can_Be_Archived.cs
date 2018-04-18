using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;

namespace Product.Tests.MailOnlyTests.MigrationTests
{
    [TestClass]
    public class TC23090_UI_Verify_Jobs_Can_Be_Archived: LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifyJobsCanBeArchived()
        {
            string login = configurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = configurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = configurator.GetValueByXpath("//metaname[text()='client1']/../name");
            string projectName = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//name");
            string sourceMailbox9 = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project1']/..//metaname[text()='entry9']/..//source");

            try
            {
                ExcelReader reader = new ExcelReader(@"C:\Users\valery.piniazik\Desktop\GroupSyncJob686Logs.csv");
                bool FileHasMessage= reader.IsTextExistDoc("Created a DirSync job");
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().SyncUserByLocator(sourceMailbox9);
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().AssertUserHaveSyncingState(sourceMailbox9);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox9);
                User.AtUsersForm().VerifyStateIS("Syncing");
                User.AtUsersForm().WaitForJobIsCreated();
                User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
                User.AtUsersForm().WaitForSyncedState();
                User.AtUsersForm().CloseUserDetails();                
                User.AtUsersForm().SelectAction(ActionType.Archive);
                User.AtUsersForm().Apply();
                User.AtUsersForm().ConfirmArchive();
                User.AtUsersForm().VerifyLineNotExist(sourceMailbox9);

            }
            catch (Exception ex)
            {
                LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw ex;
            }
        }
    }
}
