﻿using System;
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
		public void Automation_IN_PS_DistributionGroupsInitialTest()
		{
		    string userName = RunConfigurator.GetUserLogin("client2");
		    string password = RunConfigurator.GetPassword("client2");
		    string client = RunConfigurator.GetRole("client2");
            string project = RunConfigurator.GetProjectName("client2","project2");
            string group1Name = RunConfigurator.GetGroupName("client2","project2","group1");
            string group2Name = RunConfigurator.GetGroupName("client2","project2","group2");
            string group3Name = RunConfigurator.GetGroupName("client2","project2","group3");
            string group4Name = RunConfigurator.GetGroupName("client2","project2","group4");
            string group5Name = RunConfigurator.GetGroupName("client2","project2","group5");
            string group6Name = RunConfigurator.GetGroupName("client2","project2","group6");
		    string sourceLogin = RunConfigurator.GetTenantValue("T5->T6", "source", "aduser");
		    string sourcePassword = RunConfigurator.GetTenantValue("T5->T6", "source", "adpassword");
            string group3Member1 = RunConfigurator.GetGroupFirstMember("client2","project2","group3");
            string group3Owner = RunConfigurator.GetGroupOwner("client2","project2","group3");
		    string sourceUri = RunConfigurator.GetTenantValue("T5->T6", "source", "uri");

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
		            Browser.GetDriver().Navigate().Refresh();
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
		        LogHtml(Browser.GetDriver().PageSource);
                throw;
            }
		}
	}
}
