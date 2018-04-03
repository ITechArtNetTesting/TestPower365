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
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string adGroup2 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='adgroup2']/../name");

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
