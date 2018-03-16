using System;
using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using ProbeTests.Model;
using Product.Framework;
using System.Data.SqlClient;
//using BTCloud.Lib;
using log4net;
using log4net.Config;

namespace ProbeTests.ProbeTests
{
    class DiscoveryQueueTest : IProbeTest
    {
        private string connectionString;
        private int threshold;
        private string instanceName;
        private ILog Log;

        public DiscoveryQueueTest()
        {
            RunConfigurator.RunPath = "resources/probeRun.xml";
            //connectionString = RunConfigurator.GetValueByXpath("//DiscoveryQueueProbe/@connectionString").DecryptChk();
            threshold = int.Parse(RunConfigurator.GetValueByXpath("//DiscoveryQueueProbe/@threshold"));
            instanceName = ConfigurationManager.AppSettings.Get("Instance");
            XmlConfigurator.Configure();
            Log = LogManager.GetLogger(typeof(DiscoveryQueueTest));
        }

        public void Run()
        {
            Probe probe = new Probe
            {
                ProbeType = ProbeType.DiscoveryQueue,
                Started = DateTime.UtcNow,
                Instance = instanceName.ToLower(),
            };

            try
            {
                Log.Info("Checking the discovery queue");
                int? discoveryQueueSize = GetDiscoveryQueueSize();
                Log.Info(string.Format("Discovery queue checked - {0} elements", discoveryQueueSize.ToString()));

                if (discoveryQueueSize > threshold)
                {
                    probe.IsSuccess = false;
                    probe.ErrorText = "Size of the queue is " + discoveryQueueSize.ToString();
                }
                else
                {
                    probe.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                probe.IsSuccess = false;
                probe.ErrorText = e.Message;
            }

            probe.Completed = DateTime.UtcNow;

            SaveProbe(probe);
        }

        private int? GetDiscoveryQueueSize()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
            CloudQueue queue = queueClient.GetQueueReference("discoveryapplication-objects");

            queue.FetchAttributes();

            return queue.ApproximateMessageCount;
        }

        private void SaveProbe(Probe probe)
        {
            //using (var probesDB = new ProbesDb())
            //{
            //    probesDB.Probes.Add(probe);
            //    probesDB.SaveChanges();

            //    probesDB.Database.ExecuteSqlCommand(
            //        "dbo.ArchiveProbes @ProbeType, @InstanceName",
            //        new SqlParameter("@ProbeType", ProbeType.DiscoveryQueue),
            //        new SqlParameter("@InstanceName", instanceName));
            //}
        }

        public void SetUp() { }

        public void TearDown() { }
    }
}
