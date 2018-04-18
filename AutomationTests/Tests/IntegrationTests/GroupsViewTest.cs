using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
   public class GroupsViewTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public void Automation_IN_GroupsViewTest()
        {
            string userName = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetRole("client2");
            string project = RunConfigurator.GetProjectName("client2","project2");
            string adGroup2 = RunConfigurator.GetADGroupName("client2","project2","adgroup2");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                User.AtProjectOverviewForm().OpenTotalGroups();
                if (!User.AtGroupsMigrationForm().IsLineExist(adGroup2))
                {
                    User.AtGroupsMigrationForm().PerformSearch(adGroup2);
                }
                User.AtGroupsMigrationForm().SelectEntryBylocator(adGroup2);
                User.AtGroupsMigrationForm().SelectAction(ActionType.AddToWave);
                User.AtGroupsMigrationForm().AssertApplyIsDisabled();
                User.AtGroupsMigrationForm().SelectAction(ActionType.AddToProfile);
                User.AtGroupsMigrationForm().AssertApplyIsDisabled();
            }
            catch (Exception)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }

    }
}
