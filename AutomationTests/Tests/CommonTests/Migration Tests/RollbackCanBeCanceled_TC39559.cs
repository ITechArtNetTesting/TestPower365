using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using System;

namespace Product.Tests.CommonTests.Migration_Tests
{
    [TestClass]
   public class RollbackCanBeCanceled_TC39559 : LoginAndConfigureTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_Integrat_39559()
        {
            bool isIntegrate = true;
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
                RunConfigurator.GetProjectName("client2", "project2"), RunConfigurator.GetSourceMailbox("client2", "project2", "entry19"), isIntegrate);
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_MO_39559()
        {
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client1"),
                RunConfigurator.GetProjectName("client1", "project1"), RunConfigurator.GetSourceMailbox("client1", "project1", "entry13"));
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("UI")]
        public void RollbackTest_InTheUserDetailView_MD_39559()
        {
            RollbackTest_InTheUserDetailView(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
              RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetSourceMailbox("client2", "project1", "entry10"));
        }


        public void RollbackTest_InTheUserDetailView(String login, String password, String client, String projectName, String sourceMailbox, bool isIntegrate = false)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(sourceMailbox);
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);          
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
          
            //prepared
            if (isIntegrate)
            {
                User.AtUsersForm().PrepareFromDetails();
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Preparing, 2400000, 10);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Prepared, 2400000, 30);              
            }
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Syncing, 2400000, 10);
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Synced, 2400000, 10);
            User.AtUsersForm().Rollback();
            //Modal window       
            User.AtUsersForm().SetDontResetPermissions();
            User.AtUsersForm().SetSureCheckbox();
            User.AtUsersForm().RollbackCancelClick_modalWindow();
            User.AtUsersForm().RefreshData();
            //Verify
            User.AtUsersForm().AssertRollBackJobNotStarted();
            User.AtUsersForm().DetailsClose();
            //Verify
            User.AtUsersForm().PerformSearch(sourceMailbox);
            User.AtUsersForm().AssertState(State.Synced);



        }
    }

}
