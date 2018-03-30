using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Tests.CommonTests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Framework;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
    public class ArchiveProjectTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod]
        public void ArchiveProject()
        {
            string userName = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//user");
            string password = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//password");
            string client = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/../name");
            string project = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//name");
            string sourceMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//source");
            string sourceMailbox14 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry14']/..//source");
            string sourceMailbox15 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry15']/..//source");
            string targetLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
            string targetPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string sourceLogin = RunConfigurator.GetTenantValue("T5->T6", "source", "user");
            string sourcePassword = RunConfigurator.GetTenantValue("T5->T6", "source", "password");
            string targetMailbox13 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//target");
            string targetMailbox13Smtp = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//targetsmtp");
            string targetMailbox13X500 = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='entry13']/..//targetx500");
            string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
            string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");
            string targetOnPremUri = RunConfigurator.GetTenantValue("T5->T6", "target", "uri");
            LoginAndSelectRole(userName, password, client);
            SelectProject(project);
            User.AtProjectOverviewForm().OpenDiscoveryOverview();
            User.AtDiscoveryOverviewForm.DisableTenants();
            User.AtDiscoveryOverviewForm.BackToDashboard();
            User.AtProjectOverviewForm().OpenEditPublicFolders();
            User.AtPublicFoldersOverviewForm.SelectFirstPublicFolder();
            User.AtPublicFoldersOverviewForm.PerformArchiveAction();
            User.AtPublicFoldersOverviewForm.ApplyChangies();
            User.AtPublicFoldersOverviewForm.BackToDashboard();
            User.AtProjectOverviewForm().GoToStartPage();
            ArchiveProject(project);
        }
    }
}
