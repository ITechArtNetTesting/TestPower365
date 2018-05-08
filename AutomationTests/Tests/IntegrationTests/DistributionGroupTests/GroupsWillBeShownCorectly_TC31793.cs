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
    [TestClass]
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
        string disrtibutionGroup;
        string securityGroup;

    [TestInitialize()]
        public void Initialize()
        {
            login = RunConfigurator.GetUserLogin("client2");
            password = RunConfigurator.GetPassword("client2");
            client = RunConfigurator.GetClient("client2");
            project = RunConfigurator.GetProjectName("client2", "project2");
            disrtibutionGroup = RunConfigurator.GetADGroupName("client2", "project2", "group1");
            securityGroup = RunConfigurator.GetADGroupName("client2", "project2", "adgroup1");

        }

        [TestMethod]
        [TestCategory("Integration")]
        public void GroupsWillBeShownCorectly_31793()
        {

            LoginAndSelectRole(login, password, client);
            SelectProject(project);
            User.AtProjectOverviewForm().OpenTotalGroups();
            User.AtGroupsMigrationForm().SearchGroup(securityGroup);
            User.AtGroupsMigrationForm().CheckSyncIsDisabledForGroup(securityGroup);
            User.AtGroupsMigrationForm().SearchGroup(disrtibutionGroup);
            User.AtGroupsMigrationForm().CheckSyncIsEnabledForGroup(disrtibutionGroup);

        }

    }
}
