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
    public class DiscoveryFrequencyShouldBeSetted_TC32698 : LoginAndConfigureTest
    {

        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("UI")]
        public void DiscoveryFrequencyCanBeSetted_Integration_32698()
        {
            VerifyDiscoveryFrequencyCanBeSetThroughTheDiscoveryPage(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"),
                RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project2"));
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("UI")]
        public void DiscoveryFrequencyCanBeSetted_MD_32698()
        {
            VerifyDiscoveryFrequencyCanBeSetThroughTheDiscoveryPage(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"),
                RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project1"));
        }

       
        private void VerifyDiscoveryFrequencyCanBeSetThroughTheDiscoveryPage(String login, String password, String client, String projectName)
        {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().EditTenants();
                User.AtTenantsConfigurationForm().OpenDiscoveryTab();
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
                User.AtDiscoveryOverviewForm().ChangeDiscoveryFrequencyHours(1);
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
                User.AtDiscoveryOverviewForm().ChangeDiscoveryFrequencyHours(24);
                User.AtDiscoveryOverviewForm().VerifyDiscoveryFrequencyHoursMatchesDisplayedNumber(client);
           
        }

    }
}
