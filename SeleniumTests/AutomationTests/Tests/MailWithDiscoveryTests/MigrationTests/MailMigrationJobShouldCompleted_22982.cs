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
   public class MailMigrationJobShouldCompleted_22982 : LoginAndConfigureTest
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
        string sourceMailbox5;

        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            projectName = RunConfigurator.GetProjectName("client2", "project1");
            sourceMailbox5 = RunConfigurator.GetSourceMailbox("client2","project1","entry5");
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")] 
        public void MigrationJobShouldCompleted_inDetailView_22982()
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().OpenDetailsByLocator(sourceMailbox5);
            //Run Syncing
            User.AtUsersForm().SyncFromDetails();
            User.AtUsersForm().ConfirmSync();
            //Verify Syncing         
            User.AtUsersForm().WaitForState_DetailWindow(sourceMailbox5, State.Syncing, 600000, 10);
            //Synced
            User.AtUsersForm().WaitForState_DetailWindow(sourceMailbox5, State.Synced, 600000, 10);
            //Run Complete
            User.AtUsersForm().CompleteSync();
            User.AtUsersForm().ConfirmComplete();
            //Verify Complete
            User.AtUsersForm().WaitForState_DetailWindow(sourceMailbox5, State.Complete, 600000, 10);         

        }

    }
}
