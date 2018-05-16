using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.MailWithDiscoveryTests.LandingPageTests
{
    [TestClass]
    public class ProjectLandingPageShouldHasTenantSection_25771 : LoginAndConfigureTest
    {
        
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
     
      
        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        [TestCategory("UI")]
        public void ProjectLandingPageHasTenantStatusSection_MD_25117()
        {
            ProjectLandingPageHasTenantStatusSection(RunConfigurator.GetUserLogin("client2"), RunConfigurator.GetPassword("client2"), RunConfigurator.GetClient("client2"), RunConfigurator.GetProjectName("client2", "project1"));                    
        }

        
        private void ProjectLandingPageHasTenantStatusSection(String login, String password, String client, String projectName)
        {
            LoginAndSelectRole(login, password, client);
            SelectProject(projectName);
            User.AtProjectOverviewForm().AssertPageHasTenantStatusSection();

        }

    }
}
