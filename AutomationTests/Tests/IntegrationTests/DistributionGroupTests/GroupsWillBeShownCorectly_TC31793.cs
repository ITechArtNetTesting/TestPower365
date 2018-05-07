using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests.DistributionGroupTests
{
    public class GroupsWillBeShownCorectly_TC31793 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        private string login;
        private string password;
        private string client;
        private string project;
       

        [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            project = RunConfigurator.GetProjectName("client2", "project2");
            
        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GroupsWillBeShownCorectly_31793()
        {
            
                LoginAndSelectRole(login, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenTotalGroups();
                User.AtGroupsMigrationForm().ClickOnFilter();
                User.AtGroupsMigrationForm().ClickSecurityRadio();
                User.AtGroupsMigrationForm().ClickOnFilter();
                User.AtGroupsMigrationForm().CheckSyncIsDisabledForGroups();
                User.AtGroupsMigrationForm().ClickOnFilter();
                User.AtGroupsMigrationForm().ClickDistributionRadio();
                User.AtGroupsMigrationForm().ClickOnFilter();
                User.AtGroupsMigrationForm().CheckSyncIsEnabledForGroups();
            
        }

    }
}
