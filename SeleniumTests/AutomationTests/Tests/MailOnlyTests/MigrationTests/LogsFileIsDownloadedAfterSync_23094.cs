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
    public class LogsFileIsDownloadedAfterSync_TC23094 : LoginAndConfigureTest
    {
        private string userName;
        private string password;
        private string client;
        private string project;
        private string sourceMailbox9;


        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestInitialize()]
        public void Initialize()
        {
            userName = RunConfigurator.GetUserLogin("client1");
            password = RunConfigurator.GetPassword("client1");
            client = RunConfigurator.GetClient("client1");
            project = RunConfigurator.GetProjectName("client1", "project1");
            sourceMailbox9 = RunConfigurator.GetSourceMailbox("client1", "project1", "entry9");
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void LogsFileIsDownloadedAfterSync_23094()
        {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().GetUsersCount();
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerfomeActionForUser(sourceMailbox9, Framework.Enums.ActionType.Sync);
                User.AtUsersForm().ConfirmSync();
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox9);
                User.AtUsersForm().WaitForState_DetailPage(sourceMailbox9, State.Synced, 900000, 10);
                User.AtUsersForm().DownloadLogs();
                RunConfigurator.CheckLogsFileIsDownloaded();
               
            
        }

    }
}
