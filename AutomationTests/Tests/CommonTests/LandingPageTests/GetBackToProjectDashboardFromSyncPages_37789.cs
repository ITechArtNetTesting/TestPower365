using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.LandingPageTests
{
    [TestClass]
    public class GetBackToProjectDashboardFromSyncPages_37789 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("UI")]
        public void ShouldGetBackToProjectDashboardFromSyncPages_MD_37789()
        {
            ShouldGetBackToProjectDashboardFromSyncPages(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"),
               RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project1"));
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        [TestCategory("UI")]
        public void ShouldGetBackToProjectDashboardFromSyncPages_MO_37789()
        {
            ShouldGetBackToProjectDashboardFromSyncPages(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"),
               RunConfigurator.GetClient("client1"), RunConfigurator.GetProjectName("client1", "project1"));
        }
        [TestMethod]
        [TestCategory("Integration")]
        [TestCategory("UI")]
        public void ShouldGetBackToProjectDashboardFromSyncPages_Integration_37789()
        {
            ShouldGetBackToProjectDashboardFromSyncPages(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"),
               RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project2"));
        }

        private void ShouldGetBackToProjectDashboardFromSyncPages(String login, String password, String client, String projectName)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().ClickSyncNow();
            User.AtSyncNowForm.GoBack();
            User.AtProjectOverviewForm().AssertUserAtCurrentPage();
            User.AtProjectOverviewForm().ClickScheduleSync();
            User.AtSyncScheduleForm().GoBack();
            User.AtProjectOverviewForm().AssertUserAtCurrentPage();

        }

    }
}
