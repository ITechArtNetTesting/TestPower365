using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.Menu_Tests
{
    [TestClass]
    public class Verify_Users_View_has_the_Tab_to_switch_view_to_Migration_Wave_View_TC30917: LoginAndConfigureTest
    {
        [TestMethod]
        [TestCategory("MailOnly")]
        public void VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveViewForMailOnly()
        {
            string login = RunConfigurator.GetUserLogin("client1");
            string password = RunConfigurator.GetPassword("client1");
            string client = RunConfigurator.GetClient("client1");
            string projectName = RunConfigurator.GetProjectName("client1", "project1");
            VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveView(login, password, client, projectName);
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveViewForMailWithDiscovery()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project1");
            VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveView(login, password, client, projectName);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveViewForIntegration()
        {
            string login = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2", "project2");
            VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveView(login, password, client, projectName);
        }

        private void VerifyUsersViewHasTheTabToSwitchViewToMigrationWaveView(string login,string password,string client,string projectName)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenUsersList();
            User.AtUsersForm().CheckMigrationWavesIsPresent();
        }
    }
}
