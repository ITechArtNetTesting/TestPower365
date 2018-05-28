
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.LandingPageTests
{
    [TestClass]
    public class UsersViewHasTheTabToSwitchToMigrationWaveView_TC30917 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MO_30917()
        {
            string login = RunConfigurator.GetUserLogin("client1");
            string password = RunConfigurator.GetPassword("client1");
            string client = RunConfigurator.GetClient("client1");
            string projectName = RunConfigurator.GetProjectName("client1", "project1");
            UsersViewHasTheTabToSwitchToMigrationWaveView(login, password, client, projectName);
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_MD_30917()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project1");
            UsersViewHasTheTabToSwitchToMigrationWaveView(login, password, client, projectName);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void UsersViewHasTheTabToSwitchToMigrationWaveView_Integration_30917()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project2");
            UsersViewHasTheTabToSwitchToMigrationWaveView(login, password, client, projectName);
        }

        private void UsersViewHasTheTabToSwitchToMigrationWaveView(string login, string password, string client, string projectName)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().CheckMigrationWavesIsVisible();
            User.AtUsersForm().OpenMigrationwaves();
            User.AtUsersForm().CheckNewMigrationWaveButtonIsVisible();
        }

    }
}
