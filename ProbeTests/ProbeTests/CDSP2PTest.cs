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
            clientName = configurator.GetValueByXpath("//CDSP2P/@client");
            tenants = configurator.GetValueByXpath("//CDSP2P/@tenants");
            projectName = configurator.GetValueByXpath("//CDSP2P/@project");
        }

        public override void Run()
        {
            var sourceLocalLogin = configurator.GetTenantValue(tenants, "source", "aduser");
            var sourceLocalPassword = configurator.GetTenantValue(tenants, "source", "adpassword");
            var sourceLocalExchangePowerShellUri = configurator.GetTenantValue(tenants, "source", "uri");

            var sourceAzureAdSyncLogin = sourceLocalLogin;
            var sourceAzureAdSyncPassword = sourceLocalPassword;
            var sourceAzureAdSyncServer = configurator.GetTenantValue(tenants, "source", "azureAdSyncServer");

            var sourceCloudLogin = configurator.GetTenantValue(tenants, "source", "user");
            var sourceCloudPassword = configurator.GetTenantValue(tenants, "source", "password");

            var targetLocalLogin = configurator.GetTenantValue(tenants, "target", "aduser");
            var targetLocalPassword = configurator.GetTenantValue(tenants, "target", "adpassword");
            var targetLocalExchangePowerShellUri = configurator.GetTenantValue(tenants, "target", "uri");

            var targetAzureAdSyncLogin = targetLocalLogin;
            var targetAzureAdSyncPassword = targetLocalPassword;
            var targetAzureAdSyncServer = configurator.GetTenantValue(tenants, "target", "azureAdSyncServer");

            var targetCloudLogin = configurator.GetTenantValue(tenants, "target", "user");
            var targetCloudPassword = configurator.GetTenantValue(tenants, "target", "password");

            var testObjectNamePrefix = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//objectprefix");
            var testObjectOU = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//ou");


            var testObjectUPNSuffix = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//source");
            var testObjectPassword = configurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//password");
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