using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.IntegrationTests
{
    [TestClass]
   public class DistributionGroupsValidationTest : LoginAndConfigureTest
    {
        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            _testContext = testContext;
        }
        [TestMethod]
        [TestCategory("Integration_test")]
        public void Automation_IN_PS_DistributionGroupsValidationTest()
        {
            string userName = RunConfigurator.GetUserLogin("client2");
            string password = RunConfigurator.GetPassword("client2");
            string client = RunConfigurator.GetClient("client2");
            string project = RunConfigurator.GetProjectName("client2", "project2");              
            string sourceTenantName = RunConfigurator.GetTenantValue("T5->T6", "source", "name");
            string targetTenantName = RunConfigurator.GetTenantValue("T5->T6", "target", "name");
            string group1Name = RunConfigurator.GetADGroupName("client2", "project2", "group1");
            string group2Name = RunConfigurator.GetADGroupName("client2", "project2", "group2");
            string group3Name = RunConfigurator.GetADGroupName("client2", "project2", "group3");
            string group4Name = RunConfigurator.GetADGroupName("client2", "project2", "group4");
            string group5Name = RunConfigurator.GetADGroupName("client2", "project2", "group5");
            string group6Name = RunConfigurator.GetADGroupName("client2", "project2", "group6");
            string targetCloudLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "user");
            string targetCloudPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "password");
            string group2Mail = RunConfigurator.GetMail("client2", "project2", "group2");
            string group1Member1 = RunConfigurator.GetGroupMember("client2", "project2", "group1", "member1");              
            string group1Owner = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='group1']/..//owner");
            string group3Member1 = RunConfigurator.GetGroupMember("client2", "project2", "group3", "member1");           
            string group3Owner = RunConfigurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='group3']/..//owner");
            string targetOnPremLogin = RunConfigurator.GetTenantValue("T5->T6", "target", "aduser");
            string targetOnPremPassword = RunConfigurator.GetTenantValue("T5->T6", "target", "adpassword");
            string targetOnPremUri = RunConfigurator.GetTenantValue("T5->T6", "target", "uri");

            try
            {
                LoginAndSelectRole(userName, password, client);
                SelectProject(project);
                //NOTE: Discovery
                User.AtProjectOverviewForm().EditTenants();
                User.AtTenantsConfigurationForm().OpenDiscoveryTab();
                User.AtTenantsConfigurationForm().RunDiscovery(sourceTenantName);
                User.AtTenantsConfigurationForm().RunDiscovery(targetTenantName);
                User.AtTenantsConfigurationForm().GoToDashboard();
                User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(30, sourceTenantName);
                User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(30, targetTenantName);
                User.AtProjectOverviewForm().OpenMigrationGroups();
                //NOTE: Second sync
                User.AtGroupsMigrationForm().SyncUserByLocator(group1Name);
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().WaitForState(group1Name, State.Syncing, 10000);
                User.AtGroupsMigrationForm().SyncUserByLocator(group2Name);
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().WaitForState(group2Name, State.Syncing, 10000);
                User.AtGroupsMigrationForm().SyncUserByLocator(group3Name);
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().WaitForState(group3Name, State.Syncing, 10000);
                User.AtGroupsMigrationForm().SyncUserByLocator(group4Name);
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().SyncUserByLocator(group5Name);
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().WaitForState(group5Name, State.Syncing, 10000);
                User.AtGroupsMigrationForm().SyncUserByLocator(group6Name); 
                User.AtGroupsMigrationForm().Confirm();
                User.AtGroupsMigrationForm().WaitForState(group6Name, State.Syncing, 10000);
                User.AtGroupsMigrationForm().WaitForState(group1Name, State.Complete, 600000, 10);
                User.AtGroupsMigrationForm().WaitForState(group2Name, State.Complete, 600000, 10);
                User.AtGroupsMigrationForm().WaitForState(group3Name, State.Complete, 600000, 10);
                User.AtGroupsMigrationForm().WaitForState(group4Name, State.Complete, 600000, 10);
                User.AtGroupsMigrationForm().WaitForState(group5Name, State.Complete, 600000, 10);
                User.AtGroupsMigrationForm().WaitForState(group6Name, State.Complete, 600000, 10);
                Thread.Sleep(2700000);
                bool tc31803 = false;
                bool tc32395 = false;
                bool tc31805 = false;
                bool tc31806 = false;
                bool tc31814 = false;
                bool tc31818 = false;
                bool tc32493 = false;
                bool tc31808 = false;
                bool tc32281 = false;
                using (
                    var process = new PsLauncher().LaunchPowerShellInstance("IntegrationGroups-Sync.ps1",
                        $" -slogin {targetCloudLogin}" +
                        $" -spassword {targetCloudPassword}" +
                        $" -SourceGrp1 {group1Name}" +
                        $" -SourceGrp1TargetMail {group2Mail}" +
                        $" -SourceGrp1TargetMember {group1Member1}" +
                        $" -SourceGrp2 {group2Name}" +
                        $" -SourceGrp2TargetOwner {group1Owner}" +
                        $" -SourceGrp3 {group3Name}" +
                        $" -SourceGrp4 {group4Name}" +
                        $" -SourceGrp5 {group5Name}" +
                        $" -SourceGrp6 {group6Name}" +
                        $" -SourceGrp3TargetMember {group5Name}" +
                        $" -SourceGrp3TargetMemberRemove {group3Member1}" +
                        $" -SourceGrp3TargetOwnerRemove {group3Owner}" +
                        $" -adlogin {targetOnPremLogin}" +
                        $" -adpassword {targetOnPremPassword}" +
                        $" -uri {targetOnPremUri}",
                        "x64"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("TC31803 Passed"))
                        {
                            tc31803 = true;
                        }
                        if (line.Contains("TC31805 Passed"))
                        {
                            tc31805 = true;
                        }
                        if (line.Contains("TC31806 Passed"))
                        {
                            tc31806 = true;
                        }
                        if (line.Contains("TC31806 Passed"))
                        {
                            tc31806 = true;
                        }
                        if (line.Contains("TC31814 Passed"))
                        {
                            tc31814 = true;
                        }
                        if (line.Contains("TC31818 Passed"))
                        {
                            tc31818 = true;
                        }
                        if (line.Contains("TC32493 Passed"))
                        {
                            tc32493 = true;
                        }
                        if (line.Contains("TC32281 Passed"))
                        {
                            tc32281 = true;
                        }
                        if (line.Contains("TC32395 Passed"))
                        {
                            tc32395 = true;
                        }
                        if (line.Contains("TC31808 Passed"))
                        {
                            tc31808 = true;
                        }
                    }
                    process.WaitForExit(600000);
                }
                Assert.IsTrue(tc31803, "TC 31803 Failed");
                Assert.IsTrue(tc31805, "TC 31805 Failed");
                Assert.IsTrue(tc31806, "TC 31806 Failed");
                Assert.IsTrue(tc31814, "TC 31814 Failed");
                Assert.IsTrue(tc31818, "TC 31818 Failed");
                Assert.IsTrue(tc32493, "TC 32493 Failed");
                Assert.IsTrue(tc32281, "TC 32281 Failed");
                Assert.IsTrue(tc32395, "TC 32395 Failed");
                Assert.IsTrue(tc31808, "TC 31808 Failed");
            }
            catch (Exception)
            {
                LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
        }
    }
}
