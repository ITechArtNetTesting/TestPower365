using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GraphTest;
using Microsoft.Graph;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using ProbeTests.Model;
using Product.Framework;

namespace ProbeTests.ProbeTests
{
    public enum MailboxState
    {
        Unknown,
        IsSource,
        IsTarget
    }

    public class ProbeDiscoverAndMatchingTest : ProbeTest, IProbeTest
    {
        private string clientName;
        private string projectName;
        private string adGroupName;
        private string tenants;

        public ProbeDiscoverAndMatchingTest() : base(ProbeType.Discovery)
        {
            clientName = RunConfigurator.GetValueByXpath("//DiscoveryProbe/@client");
            projectName = RunConfigurator.GetValueByXpath("//DiscoveryProbe/@project");
            adGroupName = RunConfigurator.GetValueByXpath("//DiscoveryProbe/@adgroup");
            tenants = RunConfigurator.GetValueByXpath("//DiscoveryProbe/@tenants");
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        public override void Run()
        {
            try
            {
                var sourceLocalLogin = RunConfigurator.GetTenantValue(tenants, "source", "aduser");
                var sourceLocalPassword = RunConfigurator.GetTenantValue(tenants, "source", "adpassword");
                var sourceLocalExchangePowerShellUri = RunConfigurator.GetTenantValue(tenants, "source", "uri");

                var sourceAzureAdSyncLogin = sourceLocalLogin;
                var sourceAzureAdSyncPassword = sourceLocalPassword;
                var sourceAzureAdSyncServer = RunConfigurator.GetTenantValue(tenants, "source", "azureAdSyncServer");

                var sourceCloudLogin = RunConfigurator.GetTenantValue(tenants, "source", "user");
                var sourceCloudPassword = RunConfigurator.GetTenantValue(tenants, "source", "password");
                
                var targetLocalLogin = RunConfigurator.GetTenantValue(tenants, "target", "aduser");
                var targetLocalPassword = RunConfigurator.GetTenantValue(tenants, "target", "adpassword");
                var targetLocalExchangePowerShellUri = RunConfigurator.GetTenantValue(tenants, "target", "uri");

                var targetAzureAdSyncLogin = targetLocalLogin;
                var targetAzureAdSyncPassword = targetLocalPassword;
                var targetAzureAdSyncServer = RunConfigurator.GetTenantValue(tenants, "target", "azureAdSyncServer");

                var targetCloudLogin = RunConfigurator.GetTenantValue(tenants, "target", "user");
                var targetCloudPassword = RunConfigurator.GetTenantValue(tenants, "target", "password");
                
                var testMailboxNamePrefix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//probeprefix");
                var testMailboxDestinationOU = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//dirsync//ou");

                var testMailboxSourceUpnSuffix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//source");
                var testMailboxTargetUpnSuffix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//target");
                var testMailboxPassword = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//dirsync//password");

                var p365DiscoveryGroup = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='{adGroupName}']/..//name");

                var msolUri = RunConfigurator.GetValue("o365url");
                var msolConnectParams = RunConfigurator.GetValue("msolconnectargs");

                using (var process = new PsLauncher().LaunchPowerShellInstance("NewProbeUser.ps1",
                    $" -sourceLocalLogin {sourceLocalLogin}" +
                    $" -sourceLocalPassword {sourceLocalPassword}" +
                    $" -sourceLocalExchangePowerShellUri {sourceLocalExchangePowerShellUri}" +
                    $" -sourceAzureAdSyncLogin {sourceAzureAdSyncLogin}" +
                    $" -sourceAzureAdSyncPassword {sourceAzureAdSyncPassword}" +
                    $" -sourceAzureAdSyncServer {sourceAzureAdSyncServer}" +
                    $" -sourceCloudLogin {sourceCloudLogin}" +
                    $" -sourceCloudPassword {sourceCloudPassword}" +
                    $" -targetLocalLogin {targetLocalLogin}" +
                    $" -targetLocalPassword {targetLocalPassword}" +
                    $" -targetLocalExchangePowerShellUri {targetLocalExchangePowerShellUri}" +
                    $" -targetAzureAdSyncLogin {targetAzureAdSyncLogin}" +
                    $" -targetAzureAdSyncPassword {targetAzureAdSyncPassword}" +
                    $" -targetAzureAdSyncServer {targetAzureAdSyncServer}" +
                    $" -targetCloudLogin {targetCloudLogin}" +
                    $" -targetCloudPassword {targetCloudPassword}" +
                    $" -testMailboxNamePrefix {testMailboxNamePrefix}" +
                    $" -testMailboxOU {testMailboxDestinationOU}" +
                    $" -testMailboxSourceUPNSuffix {testMailboxSourceUpnSuffix}" +
                    $" -testMailboxTargetUPNSuffix {testMailboxTargetUpnSuffix}" +
                    $" -testMailboxPassword {testMailboxPassword}" +
                    $" -p365DiscoveryGroup {p365DiscoveryGroup}" +
                    $" -msolUri {msolUri}" +
                    $" -msolConnectParams \"{msolConnectParams}\"",
                        "x64"))
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                        if (line.Contains("UserPrincipalName") && String.IsNullOrWhiteSpace(Store.ProbeSourceMailbox))
                        {
                            Store.ProbeSourceMailbox = line.Substring(line.LastIndexOf(':') + 1, line.LastIndexOf('@') - line.LastIndexOf(':') - 1).Trim();
                        }
                        else if (line.Contains("UserPrincipalName") && String.IsNullOrWhiteSpace(Store.ProbeTargetMailbox))
                        {
                            Store.ProbeTargetMailbox = line.Substring(line.LastIndexOf(':') + 1, line.LastIndexOf('@') - line.LastIndexOf(':') - 1).Trim();
                        }
                    }

                    process.WaitForExit(60000);

                    if (process.ExitCode != 0)
                        throw new Exception(string.Format("PowerShell script returned exit code: {0}", process.ExitCode));

                    if (string.IsNullOrEmpty(Store.ProbeSourceMailbox) || string.IsNullOrEmpty(Store.ProbeTargetMailbox))
                    {
                        throw new Exception("Could not create source or target user");
                    }
                }
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "PowerShell/Wait till user will be created step error");
                throw;
            }
            try
            {
                LogIn(RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//user"),
                RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//password"));
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error during login and selecting role");
                throw;
            }
            try
            {
                SelectProject(RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//name"));
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error selecting project");
                throw;
            }
            try
            {
                User.AtProjectOverviewForm().EditTenants();
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error edditing tenants");
                throw;
            }

            try
            {
                User.AtTenantsConfigurationForm().OpenDiscoveryTab();
                User.AtTenantsConfigurationForm().RunDiscovery(RunConfigurator.GetTenantValue(tenants, "source", "name"));
                User.AtTenantsConfigurationForm().RunDiscovery(RunConfigurator.GetTenantValue(tenants, "target", "name"));
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error retriggering discovery");
                throw;
            }
            try
            {
                User.AtTenantsConfigurationForm().GoToDashboard();
                User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(30, RunConfigurator.GetTenantValue(tenants, "source", "name"));
                User.AtProjectOverviewForm().WaitTillDiscoveryIsComplete(30, RunConfigurator.GetTenantValue(tenants, "target", "name"));
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Discovery error");
                throw;
            }
            try
            {
                User.AtProjectOverviewForm().OpenUsersList();
                User.AtUsersForm().PerformSearch(Store.ProbeSourceMailbox);
                int counter = 0;
                var sourceUpn = string.Format("{0}@{1}", Store.ProbeSourceMailbox, RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//source"));
                while (!User.AtUsersForm().IsLineExist(sourceUpn) && counter<35)
                {
                    Log.Info("Source mailbox is not displayed");
                    Browser.GetDriver().Navigate().Refresh();
                    counter++;
                }
                User.AtUsersForm().VerifyLineisExist(sourceUpn);
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error validating source mailbox");
                throw;
            }

            try
            {
                int counter = 0;
                var targetUpn = string.Format("{0}@{1}", Store.ProbeTargetMailbox, RunConfigurator.GetValueByXpath("//metaname[text()='client1']/..//metaname[text()='project2']/..//metaname[text()='entry1']/..//target"));
                while (!User.AtUsersForm().IsLineExist(targetUpn) && counter<35)
                {
                    Log.Info("Target mailbox is not displayed");
                    Browser.GetDriver().Navigate().Refresh();
                    counter++;
                }
                User.AtUsersForm().VerifyLineisExist(targetUpn);
            }
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "Error validating target mailbox");
                throw;
            }
                InsertDataToSql(DateTime.UtcNow);
        }

        private void WaitForAddedItems(WaitInfo source, WaitInfo target, TimeSpan waitSpan, CancellationToken token)
        {
            Stopwatch swElapsed = Stopwatch.StartNew();
            Log.Info("initializing GraphAuth for source");
            GraphAuth graphAuthSource = new GraphAuth(source.TenantId, source.AppId, source.User, source.Pwd);
            Log.Info("initializing O365Graph for source");
            O365Graph graphSource = new O365Graph(graphAuthSource);
            Log.Info("initializing GraphAuth for target");
            GraphAuth graphAuthTarget = new GraphAuth(target.TenantId, target.AppId, target.User, target.Pwd);
            Log.Info("initializing O365Graph for target");
            O365Graph graphTarget = new O365Graph(graphAuthTarget);

            User sourceUser = null;
            bool foundTarget = false;
            Group sourceGroup = null;
            bool isGroupMember = false;

            while (!token.IsCancellationRequested)
            {
                try
                {
                    if (swElapsed.Elapsed >= waitSpan)
                    {
                        throw new Exception("Timed out waiting for user");
                    }
	                if (sourceUser == null)
                    {
                        foreach (List<User> probeUsers in graphSource.FindUsersStartsWith("DisplayName", Store.ProbeSourceMailbox, token))
                        {
                            sourceUser = probeUsers.FirstOrDefault();
                            break;
                        }
                    }

                    if (sourceGroup == null)
                    {
                        sourceGroup = graphSource.FindGroupByDisplayNameOrMail(source.Group, token).Result;
                    }

                    if (!isGroupMember)
                    {
                        if (sourceUser != null && sourceGroup != null)
                        {
                            // now see if the source user is in the group
                            foreach (List<DirectoryObject> members in graphSource.GetGroupMembers(sourceGroup, token))
                            {
                                var foo = members.FirstOrDefault(x => x.Id == sourceUser.Id);
                                isGroupMember = foo != null;
                                if (isGroupMember)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (!foundTarget)
                    {
                        foreach (List<User> probeUsers in graphTarget.FindUsersStartsWith("DisplayName", Store.ProbeTargetMailbox, token))
                        {
                            foundTarget = true;
                            break;
                        }
                    }

                    if (foundTarget && isGroupMember)
                    {
                        Log.Info($"Wait elapsed: {swElapsed.Elapsed}");
                        return;
                    }

                    Task.Delay(TimeSpan.FromSeconds(20), token).Wait(token);
                }
                catch (Exception ex)
                {
					Log.Info($"Source user is: {sourceUser}");
					Log.Info($"Source group is: {sourceGroup}");
					Log.Info($"isGroupMember is: {isGroupMember}");
					Log.Info($"FoundTarget is: {foundTarget}");
					Log.Info($"Elapsed time: {swElapsed.Elapsed}");
                    if (swElapsed.Elapsed >= waitSpan) throw;
                    Log.Error("Wait exception", ex);
                }
            }
        }
    }

    public class WaitInfo
    {
        public string TenantId { get; set; }
        public string AppId { get; set; }
        public string User { get; set; }
        public string Pwd { get; set; }
        public string Group { get; set; }
    }
}
