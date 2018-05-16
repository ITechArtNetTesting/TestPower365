using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                User.AtUsersForm().PerformSearch(sourceMailbox3);
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox3);
                User.AtUsersForm().SyncFromDetails();
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox3, State.Syncing, 2700000, 10);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox3, State.Synced, 2700000, 10);
                User.AtUsersForm().SyncFromDetails();
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox3, State.Syncing, 2700000, 10);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox3, State.Synced, 2700000, 10);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox3, State.Complete, 2700000, 10);              
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
