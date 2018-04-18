using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Enums;
using Product.Tests.CommonTests;

namespace Product.Tests.IntegrationTests
{
	[TestClass]
	public class DistributionGroupsInitialTest : LoginAndConfigureTest
	{
		[ClassInitialize]
		public static void ClassInit(TestContext testContext)
		{
			_testContext = testContext;
		}
		
		[TestMethod]
        [TestCategory("Integration_test")]
        public void Automation_IN_PS_DistributionGroupsInitialTest()
        {
            string userName = configurator.GetUserLogin("client2");
            string password = configurator.GetPassword("client2");
            string client = configurator.GetClient("client2");
            string project = configurator.GetProjectName("client2","project2");
            string group1Name = configurator.GetADGroupName("client2", "project2", "group1");
            string group2Name = configurator.GetADGroupName("client2", "project2", "group2");
            string group3Name = configurator.GetADGroupName("client2", "project2", "group3");
            string group4Name = configurator.GetADGroupName("client2", "project2", "group4");
            string group5Name = configurator.GetADGroupName("client2", "project2", "group5");
            string group6Name = configurator.GetADGroupName("client2", "project2", "group6");
            string sourceLogin = configurator.GetTenantValue("T5->T6", "source", "aduser");
		    string sourcePassword = configurator.GetTenantValue("T5->T6", "source", "adpassword");
            string group3Member1 = configurator.GetGroupMember("client2", "project2", "group3", "member1");         
            string group3Owner = configurator.GetValueByXpath("//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='group3']/..//owner");
		    string sourceUri = configurator.GetTenantValue("T5->T6", "source", "uri");

		    try
		    {
		        LoginAndSelectRole(userName, password, client);
		        SelectProject(project);
                try
                {
                    User.AtProjectOverviewForm().OpenMigrationGroups();
                }
                catch (Exception)
                {
                    Log.Info("Failed to open migration groups form");
                    Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
                    User.AtProjectOverviewForm().OpenMigrationGroups();
                }
                User.AtGroupsMigrationForm().SyncUserByLocator(group1Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().WaitForState(group1Name, State.Syncing, 10000);

		        User.AtGroupsMigrationForm().SyncUserByLocator(group2Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().WaitForState(group2Name, State.Syncing, 10000);
		        User.AtGroupsMigrationForm().SyncUserByLocator(group3Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().WaitForState(group3Name, State.Syncing, 10000);
		        User.AtGroupsMigrationForm().SyncUserByLocator(group4Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().SyncUserByLocator(group5Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().WaitForState(group5Name, State.Syncing, 10000);
		        User.AtGroupsMigrationForm().SyncUserByLocator(group6Name);
		        User.AtGroupsMigrationForm().ConfirmSync();
		        User.AtGroupsMigrationForm().WaitForState(group6Name, State.Syncing, 10000);
		        User.AtGroupsMigrationForm().WaitForState(group1Name, State.Complete, 60000);
		        User.AtGroupsMigrationForm().WaitForState(group2Name, State.Complete, 60000);
		        User.AtGroupsMigrationForm().WaitForState(group3Name, State.Complete, 60000);
		        User.AtGroupsMigrationForm().WaitForState(group4Name, State.Complete, 60000);
		        User.AtGroupsMigrationForm().WaitForState(group5Name, State.Complete, 60000);
		        User.AtGroupsMigrationForm().WaitForState(group6Name, State.Complete, 60000);
		        using (
		            var deltaProcess = new PsLauncher().LaunchPowerShellInstance("IntegrationGroups-Delta.ps1",
		                $" -slogin {sourceLogin}" +
		                $" -spassword {sourcePassword}" +
		                $" -SourceGrp3 {group3Name}" +
		                $" -SourceGrp3MemberRemove {group3Member1}" +
		                $" -SourceGrp3OwnerRemove {group3Owner}" +
		                $" -uri {sourceUri}",
		                "x64"))
		        {
		            while (!deltaProcess.StandardOutput.EndOfStream)
		            {
		                var line = deltaProcess.StandardOutput.ReadLine();
		                Log.Info(line);
		            }
		            deltaProcess.WaitForExit(600000);
		        }
            }
		    catch (Exception)
		    {
		        LogHtml(Driver.GetDriver(driver.GetDriverKey()).PageSource);
                throw;
            }
		}
	}
}
