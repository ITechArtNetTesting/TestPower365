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
    public class RollbackShouldBeCompleted_InTheUserDetailView : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_Integrat_39552_39571_39558_39554_39553_39567()
        {
            bool isIntegrate = true;
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
                RunConfigurator.GetProjectName("client2", "project2"), RunConfigurator.GetSourceMailbox("client2", "project2", "entry18"), isIntegrate);
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_MO_39552_39571_39558_39554_39553_39567()
        {
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client1"),
                RunConfigurator.GetProjectName("client1", "project1"), RunConfigurator.GetSourceMailbox("client1", "project1", "entry12"));
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_MD_39552_39571_39558_39554_39553_39567()
        {
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
              RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetSourceMailbox("client2", "project1", "entry9"));
        }


        public void RollbackTest_InTheUserDetailView(String login, String password, String client, String projectName, String sourceMailbox, bool isIntegrate = false)
        {
           
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(sourceMailbox);
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
            User.AtUsersForm().SelectAction(ActionType.Rollback);
            User.AtUsersForm().AssertApplyIsDisabled();
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
            User.AtUsersForm().AssertRollBackIsDisabled();
            //prepared
            if (isIntegrate)
            {
                User.AtUsersForm().PrepareFromDetails();
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Preparing, 2400000, 10);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Prepared, 2400000, 30);
                User.AtUsersForm().AssertRollBackIsDisabled();
            }
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Syncing, 2700000, 10);
            //Verify Action button since Syncing
            User.AtUsersForm().AssertRollBackIsDisabled();
            //Rollback
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Synced, 2700000, 10);
            User.AtUsersForm().Rollback();
            //Modal window
            User.AtUsersForm().AssertRollbackModalWindowIsShown();
            User.AtUsersForm().SetDontResetPermissions();
            User.AtUsersForm().SetSureCheckbox();
            User.AtUsersForm().RollbackClick_modalWindow();
            var startTime = DateTime.Now;
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.RollbackInProgress, 600000, 30);
            User.AtUsersForm().WaitForJobIsCreated(sourceMailbox, State.RollbackInProgress, 600000, 30);
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.RollbackCompleted, 2400000, 30);
            //Verify
            User.AtUsersForm().WaitForJobIsCreated(sourceMailbox, State.RollbackCompleted, 600000, 30);
            User.AtUsersForm().JobProgressBarShouldShownCorrectProgress("Rollback Complete");
            var endTime = DateTime.Now;
            User.AtUsersForm().DownloadRollbackLogs();
            User.AtUsersForm().CheckDetailsTime(startTime, endTime);
            User.AtUsersForm().CloseUserDetails();
            //Verify downloaded
            RunConfigurator.CheckRollbackLogsFileIsDownloaded();
        }
    }
}
