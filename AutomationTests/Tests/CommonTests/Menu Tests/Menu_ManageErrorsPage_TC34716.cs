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
    public class Menu_ManageErrorsPage_TC34716 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        
        [TestMethod]
        [TestCategory("Integration")]
        public void Menu_ManageErrorsPage_Integrat_34716()
        {
              Menu_ManageErrorsPage(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
                RunConfigurator.GetProjectName("client2", "project2"));
        }

        [TestMethod]
        [TestCategory("MailOnly")]
        public void Menu_ManageErrorsPage_MO_34716()
        {
            Menu_ManageErrorsPage(RunConfigurator.GetUserLogin("client1"), RunConfigurator.GetPassword("client1"), RunConfigurator.GetClient("client1"),
                RunConfigurator.GetProjectName("client1", "project1"));
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void Menu_ManageErrorsPage_MD_34716()
        {
            Menu_ManageErrorsPage(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"),
              RunConfigurator.GetProjectName("client2", "project1"));
        }

       
        private void Menu_ManageErrorsPage(String login, String password, String client, String projectName)
        {

            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().OpenMainMenu();
            User.AtProjectOverviewForm().AtMainMenu().AssertCorrectErrorsMenuItems();

        }

    }
}
