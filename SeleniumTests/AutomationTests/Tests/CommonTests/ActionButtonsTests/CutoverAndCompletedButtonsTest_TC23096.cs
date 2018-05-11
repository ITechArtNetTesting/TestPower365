using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using System;

namespace Product.Tests.CommonTests.ActionButtonsTests
{
    [TestClass]
    public class CutoverAndCompletedButtonsTest_TC23096 : LoginAndConfigureTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void CutoverAndCompletedButtonsTest_MO_23096()
        {
            CutoverAndCompletedButtonsTest(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"),
                RunConfigurator.GetClient("client1"), RunConfigurator.GetProjectName("client1", "project1"), RunConfigurator.GetSourceMailbox("client1", "project1", "entry11"));

        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void CutoverAndCompletedButtonsTest_MD_23096()
        {
            CutoverAndCompletedButtonsTest(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"),
                RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project1"), RunConfigurator.GetSourceMailbox("client2", "project1", "entry2"));

        }

        private void CutoverAndCompletedButtonsTest(String login, String password, String client, String projectName, String sourceMailbox)
        {

            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().PerformSearch(sourceMailbox);
            //Verify on Page View
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
            User.AtUsersForm().AssertCutoverCompliteDetailsIsDisabled();
            User.AtUsersForm().CloseUserDetails();

            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
            //Verify on Migration View
            User.AtUsersForm().CheckActionIsDisabled(Framework.Enums.ActionType.Cutover);
            User.AtUsersForm().CheckActionIsDisabled(Framework.Enums.ActionType.Complete);

            User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Sync);
            User.AtUsersForm().Apply();
            User.AtUsersForm().ConfirmSync();

            //Verify on Page View
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox, State.Synced, 600000, 10);
            User.AtUsersForm().AssertCutoverCompliteDetailsIsEnabled();
            User.AtUsersForm().CloseUserDetails();

            //Verify on Migration View
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
            User.AtUsersForm().CheckActionIsEnabled(Framework.Enums.ActionType.Cutover);
            User.AtUsersForm().CheckActionIsEnabled(Framework.Enums.ActionType.Complete);

        }
    }

}
