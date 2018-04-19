﻿using System;
using System.Runtime.InteropServices;
using System.Threading;
using ProbeTests.Model;
using Product.Framework;
using Product.Framework.Enums;

namespace ProbeTests.ProbeTests
{
    public class ProbeIntegrationTest : ProbeTest, IProbeTest
	{
        private string clientName;
        private string projectName;
        private string adGroupName;
        private string tenants;

		public ProbeIntegrationTest() : base(ProbeType.Integration)
		{
            clientName  = configurator.GetValueByXpath("//IntegrationProbe/@client");
            projectName = configurator.GetValueByXpath("//IntegrationProbe/@project");
            adGroupName = configurator.GetValueByXpath("//IntegrationProbe/@adgroup");
            tenants = configurator.GetValueByXpath("//IntegrationProbe/@tenants");
		}

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
		public override void Run()
		{
			try
			{
                var sourceLocalLogin = configurator.GetTenantValue(tenants, "source", "aduser");
                var sourceLocalPassword = configurator.GetTenantValue(tenants, "source", "adpassword");
                var localExchangePowerShellUri = configurator.GetTenantValue(tenants, "source", "uri");
                
                var sourceAzureAdSyncLogin = sourceLocalLogin;
                var sourceAzureAdSyncPassword = sourceLocalPassword;
                var sourceAzureAdSyncServer = configurator.GetTenantValue(tenants, "source", "azureAdSyncServer");
                
                var sourceCloudLogin = configurator.GetTenantValue(tenants, "source", "user");
                var sourceCloudPassword = configurator.GetTenantValue(tenants, "source", "password");

                var testMailboxPassword = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//dirsync//password");
                var testMailboxUpnSuffix = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//dirsync//forest");
                var testMailboxDestinationOU = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//dirsync//ou");
                var testMailboxNamePrefix = configurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project3']/..//metaname[text()='entry1']/..//probeprefix");

                var p365DiscoveryGroup = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='{adGroupName}']/..//name");

                var msolUri = configurator.GetValue("o365url");
                var msolConnectParams = configurator.GetValue("msolconnectargs");

                using (var process = new PsLauncher(store).LaunchPowerShellInstance("Integration_Probe.ps1",
                        $" -localLogin {sourceLocalLogin}" +
                        $" -localPassword {sourceLocalPassword}" +
                        $" -localExchangePowerShellUri {localExchangePowerShellUri}" +

                        $" -azureAdSyncLogin {sourceAzureAdSyncLogin}" +
                        $" -azureAdSyncPassword {sourceAzureAdSyncPassword}" +
                        $" -azureAdSyncServer {sourceAzureAdSyncServer}" +
                        
                        $" -cloudLogin {sourceCloudLogin}" +
                        $" -cloudPassword {sourceCloudPassword}" +

                        $" -testMailboxPassword {testMailboxPassword}" +
                        $" -testMailboxUPNSuffix {testMailboxUpnSuffix}" +
                        $" -testMailboxOU {testMailboxDestinationOU}" +
                        $" -testMailboxNamePrefix {testMailboxNamePrefix}" +

                        $" -p365DiscoveryGroup {p365DiscoveryGroup}" +

                        $" -msolUri {msolUri}" +
                        $" -msolConnectParams \"{msolConnectParams}\"",
                    "x64"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("UserPrincipalName"))
                        {
                            store.ProbeSourceMailbox = line.Substring(line.LastIndexOf(':') + 1, line.LastIndexOf('@') - line.LastIndexOf(':') - 1).Trim();
                            break;
                        }
                    }
                    process.WaitForExit(60000);

                    if (process.ExitCode != 0)
                        throw new Exception(string.Format("PowerShell script returned exit code: {0}", process.ExitCode));
                }
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "PowerShell error");
				throw e;
			}
			try
			{
                LogIn(configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//user"),
                        configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//password"));
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Login error");
				throw e;
			}
			try
			{
                SelectProject(configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//name"));
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Selecting project error");
				throw e;
			}
			try
			{
				User.AtProjectOverviewForm().EditTenants();
				User.AtTenantsConfigurationForm().OpenDiscoveryTab();
                User.AtTenantsConfigurationForm().RunDiscovery(configurator.GetTenantValue(tenants, "source", "name"));
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Run discovery error");
				throw e;
			}
			try
			{
				User.AtTenantsConfigurationForm().GoToDashboard();
				User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(30, configurator.GetTenantValue(tenants, "source", "name"));
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Wait for discovery is completed error");
				throw e;
			}
			try
			{
				User.AtProjectOverviewForm().OpenUsersList();
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Navigating to user migration view error");
				throw e;
			}
			try
			{
				User.AtUsersForm().PerformSearch(store.ProbeSourceMailbox);
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Search error");
				throw e;
			}
			string sourceMailbox = store.ProbeSourceMailbox.Trim() +
                                   configurator.GetValueByXpath(
                                       $"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//source");
			try
			{
				int counter = 0;
				while (!User.AtUsersForm().IsLineExist(sourceMailbox) && counter<35)
				{
					Log.Info("Source mailbox is not displayed");
					Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
					counter++;
				}
					User.AtUsersForm().VerifyLineisExist(sourceMailbox);
					User.AtUsersForm().WaitForState(sourceMailbox, State.NoMatch, 30000);
				}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Verifying mailbox is displayed error");
				throw e;
			}
			try
			{
			    try
			    {
			        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                }
			    catch (Exception )
			    {
			       Log.Info("Error selecting mailbox, retrying");
			       User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
                }
			    User.AtUsersForm().SelectAction(ActionType.Prepare);
                try
				{
                    User.AtUsersForm().Apply();
				}
				catch (Exception)
				{
					Log.Info("Apply button is not enabled");
					Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
					User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
					User.AtUsersForm().SelectAction(ActionType.Prepare);
					User.AtUsersForm().Apply();
				}
			    try
			    {
			        User.AtUsersForm().ConfirmPrepare();
                }
			    catch (Exception)
			    {
			        Driver.GetDriver(driver.GetDriverKey()).Navigate().Refresh();
			        User.AtUsersForm().SelectEntryBylocator(sourceMailbox);
			        User.AtUsersForm().SelectAction(ActionType.Prepare);
			        User.AtUsersForm().Apply();
                    User.AtUsersForm().ConfirmPrepare();
                }
				
			}
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Preparing error");
				throw e;
			}
            
            try
			{
                User.AtUsersForm().OpenDetailsByLocator(sourceMailbox);
                int count = 0;
                int maxCount = 60;

                while(!User.AtUsersForm().IsElementPresent("Job Finishing Label", "//*[contains(text(), 'Finishing')]") && count < maxCount)
                {
                    Thread.Sleep(60000);
                    User.AtUsersForm().DetailsRefresh();
                    count++;
                }

                User.AtUsersForm().DetailsClose();

                if (count < maxCount)
                {
                    try
                    {
                        var targetAzureAdSyncLogin = configurator.GetTenantValue(tenants, "target", "aduser");
                        var targetAzureAdSyncPassword = configurator.GetTenantValue(tenants, "target", "adpassword");
                        var targetAzureAdSyncServer = configurator.GetTenantValue(tenants, "target", "azureAdSyncServer");

                        using (var process = new PsLauncher(store).LaunchPowerShellInstance("ADSync.ps1",
                                    $" -login {targetAzureAdSyncLogin}" +
                                    $" -password {targetAzureAdSyncPassword}" +
                                    $" -server {targetAzureAdSyncServer}",
                                "x64"))
                        {
                            while (!process.StandardOutput.EndOfStream)
                            {
                                var line = process.StandardOutput.ReadLine();
                                Log.Info(line);
                            }

                            process.WaitForExit(60000);

                            if (process.ExitCode != 0)
                                throw new Exception(string.Format("PowerShell script returned exit code: {0}", process.ExitCode));
                        }
                    }
                    catch (Exception e)
                    {
                        InsertDataToSql(DateTime.UtcNow, "Target Azure AD Sync Failed.");
                        throw e;
                    }
                }

                User.AtUsersForm().WaitForState(sourceMailbox, State.Prepared, 90000);
            }
			catch (Exception e)
			{
				InsertDataToSql(DateTime.UtcNow, "Wait for Prepared state error");
				throw e;
			}
			InsertDataToSql(DateTime.UtcNow);
		}
	}
}
