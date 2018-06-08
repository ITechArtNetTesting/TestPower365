using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.SetupTests
{
    [TestClass]
    public class EnvironmentCleanup: LoginAndConfigureTest
    {
        [TestMethod]
        [TestCategory("Setup")]
        public void CleanupEnvironment()
        {
            var clientName = RunConfigurator.GetClient("client2");
            var projectName = RunConfigurator.GetProjectName("client2", "project2");
            var tenants = "T5->T6";

            var sourceLocalLogin = RunConfigurator.GetTenantValue(tenants, "source", "aduser");
            var sourceLocalPassword = RunConfigurator.GetTenantValue(tenants, "source", "adpassword");
            var sourceLocalExchangePowerShellUri = RunConfigurator.GetTenantValue(tenants, "source", "uri");

            var sourceAzureAdSyncLogin = sourceLocalLogin;
            var sourceAzureAdSyncPassword = sourceLocalPassword;
            var sourceAzureAdSyncServer = RunConfigurator.GetTenantValue(tenants, "source", "azureAdSyncServer");

            var targetLocalLogin = RunConfigurator.GetTenantValue(tenants, "target", "aduser");
            var targetLocalPassword = RunConfigurator.GetTenantValue(tenants, "target", "adpassword");
            var targetLocalExchangePowerShellUri = RunConfigurator.GetTenantValue(tenants, "target", "uri");

            var targetAzureAdSyncLogin = targetLocalLogin;
            var targetAzureAdSyncPassword = targetLocalPassword;
            var targetAzureAdSyncServer = RunConfigurator.GetTenantValue(tenants, "target", "azureAdSyncServer");


            var testGroupName = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='groupreset']/..//name");
            var testGroupOwner = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='groupreset']/..//owner");
            var testGroupMember = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='groupreset']/..//member1");

            var testDistributionGroupPrefix = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='groupreset']/..//prefix");
            var testMailboxNamePrefixArray = RunConfigurator.GetValueByXpath($"//metaname[text()='client2']/..//metaname[text()='project2']/..//metaname[text()='mailboxreset']/..//prefixArray");

            var syncDelaySec = Environment.GetEnvironmentVariable("AzureADSyncDelaySec") ?? "1200";

            using (var process = new PsLauncher().LaunchPowerShellInstance("resources\\IntegrationGroups-Cleanup.ps1",
                $" -sourceLocalLogin {sourceLocalLogin}" +
                $" -sourceLocalPassword {sourceLocalPassword}" +
                $" -sourceLocalExchangePowerShellUri {sourceLocalExchangePowerShellUri}" +
                $" -sourceAzureAdSyncLogin {sourceAzureAdSyncLogin}" +
                $" -sourceAzureAdSyncPassword {sourceAzureAdSyncPassword}" +
                $" -sourceAzureAdSyncServer {sourceAzureAdSyncServer}" +
                $" -targetLocalLogin {targetLocalLogin}" +
                $" -targetLocalPassword {targetLocalPassword}" +
                $" -targetLocalExchangePowerShellUri {targetLocalExchangePowerShellUri}" +
                $" -targetAzureAdSyncLogin {targetAzureAdSyncLogin}" +
                $" -targetAzureAdSyncPassword {targetAzureAdSyncPassword}" +
                $" -targetAzureAdSyncServer {targetAzureAdSyncServer}" +
                $" -testGroupName {testGroupName}" +
                $" -testGroupOwner {testGroupOwner}" +
                $" -testGroupMember {testGroupMember}" +
                $" -testDistributionGroupPrefix {testDistributionGroupPrefix}" +
                $" -testMailboxNamePrefixArray {testMailboxNamePrefixArray}" +
                $" -azureAdSyncDelaySec {syncDelaySec}",
                //$" -simulationMode",
                    "x64"))
            {
                Task stdout = Task.Run(() =>
                {
                    while (!process.StandardOutput.EndOfStream)
                    {
                        var line = process.StandardOutput.ReadLine();
                        Log.Info(line);
                    }
                });

                Task stderr = Task.Run(() =>
                {
                    while (!process.StandardError.EndOfStream)
                    {
                        var line = process.StandardError.ReadLine();
                        Log.Error(line);
                    }
                });

                Task.WaitAll(stderr, stdout);
                process.WaitForExit(60000);

                stdout.Dispose();
                stderr.Dispose();

                if (process.ExitCode != 0)
                    throw new Exception(string.Format("PowerShell script returned exit code: {0}", process.ExitCode));
            }
        }

        [TestInitialize]
        public override void SetUp()
        {
            RunOnce();
            RunConfigurator.RunPath = "resources/run.xml";
        }
    }


}
