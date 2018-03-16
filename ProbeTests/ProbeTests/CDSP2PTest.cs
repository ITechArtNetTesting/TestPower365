using System;
using System.Globalization;
using ProbeTests.Model;
using Product.Framework;
using Product.Framework.Enums;

namespace ProbeTests.ProbeTests
{
    public class CDSP2PTest : ProbeTest, IProbeTest
    {
        private string tenants;
        private string projectName;
        private string clientName;

        public CDSP2PTest() : base(ProbeType.CDSP2P)
        {
            clientName = RunConfigurator.GetValueByXpath("//CDSP2P/@client");
            tenants = RunConfigurator.GetValueByXpath("//CDSP2P/@tenants");
            projectName = RunConfigurator.GetValueByXpath("//CDSP2P/@project");
        }

        public override void Run()
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

            var testObjectNamePrefix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//objectprefix");
            var testObjectOU = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//ou");


            var testObjectUPNSuffix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//source");
            var testObjectPassword = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//password");
            try
            {
                using (var process = new PsLauncher().LaunchPowerShellInstance("CreateAndVerify.ps1",
                    $" -sourceLocalLogin {sourceLocalLogin}" +
                    $" -sourceLocalPassword {sourceLocalPassword}" +
                    $" -sourceLocalExchangePowerShellUri {sourceLocalExchangePowerShellUri}" +
                    $" -targetLocalLogin {targetLocalLogin}" +
                    $" -targetLocalPassword {targetLocalPassword}" +
                    $" -targetLocalExchangePowerShellUri {targetLocalExchangePowerShellUri}" +
                    $" -testObjectNamePrefix {testObjectNamePrefix}" +
                    $" -testObjectOU {testObjectOU}" +
                    $" -testObjectUPNSuffix {testObjectUPNSuffix}" +
                    $" -testObjectPassword {testObjectPassword}",
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
                InsertDataToSql(DateTime.UtcNow, "PowerShell/Wait till user will be created step error");
                throw;
            }

            InsertDataToSql(DateTime.UtcNow);
        }
    }
}