using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.IntegrationTests.GroupsTests
{
    [TestClass]
    public class Verify_only_universal_distribution_groups_will_be_shown_in_the_UI_for_group_sync_and_security_group_will_be_shown_in_the_UI_but_it_can_not_be_synced_TC31793 : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration")]
        public void IntegrationProVerifyOnlyMailUPNAndExtensionAttributesWillBeValidMatchingOptionsForUsersDuringProjectWizard()
        {
            string login = RunConfigurator.GetUserLogin("client1");
            string password = RunConfigurator.GetPassword("client1");
            string client = RunConfigurator.GetClient("client2");
            string projectName = RunConfigurator.GetProjectName("client2","project2");
            string disrtibutionGroup = RunConfigurator.GetADGroupName("client2","project2","group1");
            string securityGroup = RunConfigurator.GetADGroupName("client2","project2","adgroup1");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
                User.AtProjectOverviewForm().OpenTotalGroups();
                User.AtGroupsMigrationForm().SearchGroup(securityGroup);             
                User.AtGroupsMigrationForm().CheckSyncIsDisabledForGroup(securityGroup);
                User.AtGroupsMigrationForm().SearchGroup(disrtibutionGroup);                
                User.AtGroupsMigrationForm().CheckSyncIsEnabledForGroup(disrtibutionGroup);
            }
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}
