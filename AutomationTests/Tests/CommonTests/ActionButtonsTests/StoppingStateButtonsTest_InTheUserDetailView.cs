using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.ActionButtonsTests
{
    [TestClass]
    public class StoppingStateButtonsTest_InTheUserDetailView : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void StoppingStateButtonsTest_InTheUserDetailView_Integrat_25829_25830()
        {                    
            bool isIntegrate = true;
            StoppingStateButtonsTest(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
                RunConfigurator.GetProjectName("client2", "project2"), RunConfigurator.GetSourceMailbox("client2", "project2", "entry17"), isIntegrate);
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void StoppingStateButtonsTest_InTheUserDetailView_MO_25829_25830()
        {                  
            StoppingStateButtonsTest(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client1"),
                RunConfigurator.GetProjectName("client1", "project1"), RunConfigurator.GetSourceMailbox("client1", "project1", "entry4"));
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void StoppingStateButtonsTest_InTheUserDetailView_MD_25829_25830()
        {
              StoppingStateButtonsTest(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
                RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetSourceMailbox("client2", "project1", "entry7"));
        }


        private void StoppingStateButtonsTest(String login, String password, String client, String projectName, String mailbox, bool isIntegrate=false)
        {

            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(mailbox);
            User.AtUsersForm().OpenDetailsByLocator(mailbox);
            //prepared
            if (isIntegrate)
            {
                User.AtUsersForm().PrepareFromDetails();
                User.AtUsersForm().ConfirmAction();
                User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Preparing, 1200000, 10);
                User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Prepared, 1200000, 30);
            }
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Syncing, 900000, 10);
            //Verify Action button since Syncing
            User.AtUsersForm().AssertDetailsStopButtonIsEnabled();
            User.AtUsersForm().AssertDetailsSyncButtonIsDisabled();
            //Stopping
            User.AtUsersForm().StopFromDetails();
            User.AtUsersForm().ConfirmAction();
            User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Stopping, 900000, 30);
            //Verify
            User.AtUsersForm().WaitForJobIsCreated(mailbox, State.Stopped, 600000, 30);
            
            if (isIntegrate)
            {
                //verify Integration project
                User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Prepared, 600000, 30);
            }
            else {
                //verify MailOnly and Mail With Discovery project
                User.AtUsersForm().WaitForState_DetailPage(mailbox, State.Matched, 600000, 30);
            }


        }

    }
    }

