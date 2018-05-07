﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
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
    public class TC31793_Verify_only_universal_distribution_groups_will_be_shown_in_the_UI_for_group_sync_and_security_group_will_be_shown_in_the_UI_but_it_can_not_be_synced : LoginAndConfigureTest
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
            string login = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string projectName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            try
            {
                LoginAndSelectRole(login, password, client);
                SelectProject(projectName);
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
            catch (Exception e)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw e;
            }
        }
    }
}