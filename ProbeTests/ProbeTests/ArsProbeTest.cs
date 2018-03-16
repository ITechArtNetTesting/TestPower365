using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
//using BTCloud.HealthProbe;
using ProbeTests.Model;
using Product.Framework;
//using BTCloud.Monitor;
using System.Configuration;

namespace ProbeTests.ProbeTests
{
    public class ArsProbeTest : IProbeTest
    {
        private string tenantA;
        private string tenantB;

        string TenantAUid;
        string TenantADomain;
        string TenantAUser;
        string TenantAPwd;
        string TenantBUid;
        string TenantBDomain;
        string TenantBUser;
        string TenantBPwd;

        public ArsProbeTest()
        {
            RunConfigurator.RunPath = "resources/probeRun.xml";
            tenantA = RunConfigurator.GetValueByXpath("//ArsProbe/@tenantA");
            tenantB = RunConfigurator.GetValueByXpath("//ArsProbe/@tenantB");
        }

        public void Run()
        {
            //try
            //{
            //    RecipSettings TenantARecip = new RecipSettings
            //    {
            //        DisplayName = "tenantA",
            //        Email = TenantAUser,
            //        Mailbox = TenantAUser,
            //        Password = TenantAPwd
            //    };

            //    RecipSettings TenantBRecip = new RecipSettings
            //    {
            //        DisplayName = "tenantB",
            //        Email = TenantBUser,
            //        Mailbox = TenantBUser,
            //        Password = TenantBPwd
            //    };

            //    HealthProbe healthProbe = new HealthProbe();

            //    int sequence = 0;
            //    CancellationTokenSource cancel = new CancellationTokenSource();

            //    HealthProbeSettings hpSettingsAB = new HealthProbeSettings();
            //    hpSettingsAB.SequenceId = GetSequenceId(ref sequence);
            //    hpSettingsAB.Name = $"BT-IntegrationPro-{TenantAUid}-{TenantBUid}";
            //    hpSettingsAB.TestAddressSent = $"BT-IntegrationPro-Probe-{TenantAUid}-{TenantBUid}@{TenantADomain}";
            //    hpSettingsAB.TestAddressSentExpected = $"BT-IntegrationPro-Probe-{TenantAUid}-{TenantBUid}@{TenantBDomain}";
            //    hpSettingsAB.Sender = TenantARecip;
            //    hpSettingsAB.Recipient = TenantBRecip;
            //    HealthProbeResult resultAB = healthProbe.RunAsync(hpSettingsAB, null, cancel.Token).GetAwaiter().GetResult();
            //    CheckProbeResult(resultAB, ProbeType.ArsAToB);

            //    HealthProbeSettings hpSettingsBA = new HealthProbeSettings();
            //    hpSettingsBA.SequenceId = GetSequenceId(ref sequence);
            //    hpSettingsBA.Name = $"BT-IntegrationPro-{TenantBUid}-{TenantAUid}";
            //    hpSettingsBA.TestAddressSent = $"BT-IntegrationPro-Probe-{TenantAUid}-{TenantBUid}@{TenantBDomain}";
            //    hpSettingsBA.TestAddressSentExpected = $"BT-IntegrationPro-Probe-{TenantAUid}-{TenantBUid}@{TenantADomain}";
            //    hpSettingsBA.Sender = TenantBRecip;
            //    hpSettingsBA.Recipient = TenantARecip;
            //    HealthProbeResult resultBA = healthProbe.RunAsync(hpSettingsBA, null, cancel.Token).GetAwaiter().GetResult();
            //    CheckProbeResult(resultBA, ProbeType.ArsBToA);
            //}
            //catch (Exception ex)
            //{
            //    Console.Error.WriteLine(ex);
            //}
        }

        public void SetUp()
        {
            TenantAUid = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantA}']/uid");
            TenantADomain = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantA}']/domain");
            TenantAUser = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantA}']/user");
            TenantAPwd = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantA}']/password");

            CheckValues(TenantAUid, TenantADomain, TenantAUser, TenantAPwd);

            TenantBUid = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantB}']/uid");
            TenantBDomain = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantB}']/domain");
            TenantBUser = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantB}']/user");
            TenantBPwd = RunConfigurator.GetValueByXpath($"//tenants/tenant[@metaname='{tenantB}']/password");

            CheckValues(TenantBUid, TenantBDomain, TenantBUser, TenantBPwd);
        }

        public void TearDown()
        {
        }

        //private static void CheckProbeResult(HealthProbeResult result, ProbeType probeType)
        //{
        //    string instanceName = ConfigurationManager.AppSettings.Get("Instance");

        //    using (var probesDB = new ProbesDb())
        //    {
        //        Probe probe = new Probe
        //        {
        //            Completed = result.EndTimeUtc,
        //            ProbeType = probeType,
        //            ErrorText = result.ErrorText,
        //            IsSuccess = result.Succeeded,
        //            Started = result.StartTimeUtc.HasValue ? result.StartTimeUtc.Value : DateTime.MinValue,
        //            Instance = instanceName.ToLower()
        //    };

        //        Probe foo = probesDB.Probes.Add(probe);
        //        int saved = probesDB.SaveChanges();

        //        probesDB.Database.ExecuteSqlCommand(
        //            "dbo.ArchiveProbes @ProbeType, @InstanceName",
        //            new SqlParameter("@ProbeType", probeType),
        //            new SqlParameter("@InstanceName", instanceName));
        //    }
        //}

        private static string GetSequenceId(ref int sequence)
        {
            DateTime now = DateTime.UtcNow;
            return $"{now.ToString("yyMMdd:hhmmss:")}{sequence++}";
        }

        private static void CheckValues(string tenantUid, string tenantDomain, string tenantUser, string tenantPwd)
        {
            if (string.IsNullOrEmpty(tenantUid))
            {
                throw new ArgumentNullException(nameof(tenantUid));
            }

            if (string.IsNullOrEmpty(tenantDomain))
            {
                throw new ArgumentNullException(nameof(tenantDomain));
            }

            if (string.IsNullOrEmpty(tenantUser))
            {
                throw new ArgumentNullException(nameof(tenantUser));
            }

            if (string.IsNullOrEmpty(tenantPwd))
            {
                throw new ArgumentNullException(nameof(tenantPwd));
            }
        }
    }
}
