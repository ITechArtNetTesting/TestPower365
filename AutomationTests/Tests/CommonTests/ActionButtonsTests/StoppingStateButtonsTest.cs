using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.CommonTests.ActionButtonsTests
{
    [TestClass]
    public class StoppingStateButtonsTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
                
        [TestMethod]
        [TestCategory("MailOnly")]
        public void StoppingStateButtonsTest_MO_23092()
        {
           
         StoppingStateButtons(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client1"),
                                 RunConfigurator.GetProjectName("client1", "project1"), RunConfigurator.GetSourceMailbox("client1", "project1", "entry2"));
        }


        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void StoppingStateButtonsTest_mailWithDiscovery_25825()
        {
            StoppingStateButtons(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client2"),
                                 RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetSourceMailbox("client2", "project1", "entry4"));

        }

        [TestMethod]
        [TestCategory("Integration")]
        public void StoppingStateButtonsTest_Integration_25825()
        {
            bool isIntegrate = true;          
            StoppingStateButtons(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client2"),
                                RunConfigurator.GetProjectName("client2", "project2"), RunConfigurator.GetSourceMailbox("client2", "project2", "entry10"), isIntegrate);//entry10

        }

        private void StoppingStateButtons(String login, String password, String client, String projectName, String sourceMailbox, bool isIntegrate=false)
        {
           
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerformSearch(sourceMailbox);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                //For Integration project
                if (isIntegrate)
                {
                    User.AtUsersForm().SelectAction(ActionType.Prepare);
                    User.AtUsersForm().Apply();
                    User.AtUsersForm().ConfirmAction();
                    WaitForState(sourceMailbox, State.Prepared, 2500000, 30);
                    User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                }

            User.AtUsersForm().CheckActionIsEnabled(ActionType.Sync);            
                User.AtUsersForm().Apply();
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().WaitForState(sourceMailbox, State.Syncing, 2500000, 10);
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                //Verify
                User.AtUsersForm().CheckActionIsDisabled(ActionType.Sync);
                User.AtUsersForm().CheckActionIsEnabled(ActionType.Stop);            
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                User.AtUsersForm().SelectAction(ActionType.Stop);
                User.AtUsersForm().Apply();
                User.AtUsersForm().ConfirmStop();
                User.AtUsersForm().WaitForState(sourceMailbox, State.Stopping, 5000);
                //Verify
                User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                User.AtUsersForm().CheckActionIsDisabled(ActionType.Sync);                
                User.AtUsersForm().CheckActionIsDisabled(ActionType.Stop);
                if (isIntegrate)  //For Integration project
                {
                    User.AtUsersForm().CheckActionIsDisabled(ActionType.Cutover);
                }
                else  //For MailOnly and Discovery projects
                {
                    User.AtUsersForm().CheckActionIsDisabled(ActionType.Complete);
                }

        }
   }
}