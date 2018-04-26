using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailWithDiscoveryTests.MigrationTests
{
    [TestClass]
    public class MailMigrationJobShouldCompletedInUserMigrationView_25824 : LoginAndConfigureTest
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
            login = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            projectName = RunConfigurator.GetProjectName("client2", "project1");
            sourceMailbox6 = RunConfigurator.GetSourceMailbox("client2", "project1", "entry6");

        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void MigrationJobShouldCompleted_InUserMigrationView_25824()
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();

            //Syncing
            User.AtUsersForm().PerfomeActionForUser(sourceMailbox6, Framework.Enums.ActionType.Sync);
            User.AtUsersForm().ConfirmSync();
             User.AtUsersForm().WaitForState(sourceMailbox6, State.Syncing, 600000, 10);
            //Verify Synced
            User.AtUsersForm().WaitForState(sourceMailbox6, State.Synced, 600000, 10);
            //Complete
          
            User.AtUsersForm().PerfomeActionForUser(sourceMailbox6, ActionType.Complete);
            User.AtUsersForm().ConfirmComplete();
            //Verify Complete
            User.AtUsersForm().WaitForState(sourceMailbox6, State.Complete, 600000, 10);



        }

    }
}