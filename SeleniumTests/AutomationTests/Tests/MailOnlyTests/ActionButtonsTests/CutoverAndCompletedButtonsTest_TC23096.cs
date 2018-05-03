using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailOnlyTests.ActionButtonsTests
{
    [TestClass]
    public class CutoverAndCompletedButtonsTest_TC23096 : LoginAndConfigureTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        string login;
        string password;
        string client;
        string projectName;
        string sourceMailbox6;

        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client1");
            password = RunConfigurator.GetPassword("client1");
            client = RunConfigurator.GetClient("client1");
            projectName = RunConfigurator.GetProjectName("client1", "project1");
            sourceMailbox6 = RunConfigurator.GetSourceMailbox("client1", "project1", "entry6");


        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void CutoverAndCompletedButtonsTest_23096()
        {

            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();

            //Verify on Page View
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox6);
            User.AtUsersForm().AssertCutoverCompliteDetailsIsDisabled();
            User.AtUsersForm().CloseUserDetails();

            User.AtUsersForm().SelectEntryBylocator(sourceMailbox6);
            //Verify on Migration View
            User.AtUsersForm().CheckActionIsDisabled(Framework.Enums.ActionType.Cutover);
            User.AtUsersForm().CheckActionIsDisabled(Framework.Enums.ActionType.Complete);

            User.AtUsersForm().SelectAction(Framework.Enums.ActionType.Sync);
            User.AtUsersForm().Apply();
            User.AtUsersForm().ConfirmSync();

            //Verify on Page View
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox6);
            User.AtUsersForm().WaitForState_DetailPage(sourceMailbox6, State.Synced, 600000, 10);
            User.AtUsersForm().AssertCutoverCompliteDetailsIsEnabled();
            User.AtUsersForm().CloseUserDetails();

            //Verify on Migration View
            User.AtUsersForm().SelectEntryBylocator(sourceMailbox6);
            User.AtUsersForm().CheckActionIsEnabled(Framework.Enums.ActionType.Cutover);
            User.AtUsersForm().CheckActionIsEnabled(Framework.Enums.ActionType.Complete);

        }
    }
}
