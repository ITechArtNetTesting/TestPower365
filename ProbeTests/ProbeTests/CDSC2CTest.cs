using System;
using System.Threading.Tasks;
using ProbeTests.Model;
using Product.Framework;

namespace ProbeTests.ProbeTests
{
    public class CDSC2CTest : ProbeTest, IProbeTest
    {
        private readonly string tenants;
        private readonly string projectName;
        private readonly string clientName;
        private readonly string msolUri;

        public CDSC2CTest() : base(ProbeType.CDSC2C)
        {
            clientName = RunConfigurator.GetValueByXpath("//CDSC2C/@client");
            tenants = RunConfigurator.GetValueByXpath("//CDSC2C/@tenants");
            projectName = RunConfigurator.GetValueByXpath("//CDSC2C/@project");
            msolUri = RunConfigurator.GetValueByXpath("//CDSC2C/@msolUri");
        }

        public override void Run()
        {
            Log.Info("=-=-=-=-=-=-=-=-=");
            var sourceCloudLogin = RunConfigurator.GetTenantValue(tenants, "source", "user");
            var sourceCloudPassword = RunConfigurator.GetTenantValue(tenants, "source", "password");
            var sourceObjectUPNSuffix = RunConfigurator.GetTenantValue(tenants, "source", "domain");

            var targetCloudLogin = RunConfigurator.GetTenantValue(tenants, "target", "user");
            var targetCloudPassword = RunConfigurator.GetTenantValue(tenants, "target", "password");
            var targetObjectUPNSuffix = RunConfigurator.GetTenantValue(tenants, "target", "domain");

            var testObjectNamePrefix = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//objectprefix");
            var testDiscoveryGroup = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//discoverygroup");
            var testObjectPassword = RunConfigurator.GetValueByXpath($"//metaname[text()='{clientName}']/..//metaname[text()='{projectName}']/..//metaname[text()='entry1']/..//password");

            try
            {
                //  -sourceCloudLogin, 
                //  -sourceCloudPassword, 
                //  -targetCloudLogin, 
                //  -targetCloudPassword,
                //  -testObjectNamePrefix, 
                //  -testDiscoveryGroup, 
                //  -testObjectUPNSuffix, 
                //  -testObjectPassword, 
                //  -msolUri = "https://ps.outlook.com/powershell",
                //  -msolConnectParams = "",
                //  -simulationMode

                string msolConnectParams = "";
                using (var process = new PsLauncher().LaunchPowerShellInstance("resources\\CreateAndVerifyC2C.ps1",
                    $" -sourceCloudLogin \"{sourceCloudLogin}\"" +
                    $" -sourceCloudPassword \"{sourceCloudPassword}\"" +
                    $" -sourceObjectUPNSuffix \"{sourceObjectUPNSuffix}\"" +

                    $" -targetCloudLogin \"{targetCloudLogin}\"" +
                    $" -targetCloudPassword \"{targetCloudPassword}\"" +
                    $" -targetObjectUPNSuffix \"{targetObjectUPNSuffix}\"" +

                    $" -testObjectNamePrefix \"{testObjectNamePrefix}\"" +
                    $" -testDiscoveryGroup \"{testDiscoveryGroup}\"" +
                    $" -testObjectPassword \"{testObjectPassword}\"" +
                    $" -msolUri \"{msolUri}\"" +
                    $" -msolConnectParams \"{msolConnectParams}\"",
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
            catch (Exception e)
            {
                InsertDataToSql(DateTime.UtcNow, "PowerShell/Wait till user will be created step error");
                throw;
            }

            InsertDataToSql(DateTime.UtcNow);
        }

        public override void SetUp()
        {
        }
    }
}