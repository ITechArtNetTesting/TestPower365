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

        string login ;
        string password;
        string client ;
        string projectName;

       [TestInitialize()]
        public void Initialize()
        {
            login= RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            projectName = RunConfigurator.GetProjectName("client2", "project1");
          
        }

        [TestMethod]
        [TestCategory("MailWithDiscovery")]
        public void ProjectLandingPageHasTenantStatusSection_25117()
        {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().AssertPageHasTenantStatusSection();     
           
        }
    }
}
