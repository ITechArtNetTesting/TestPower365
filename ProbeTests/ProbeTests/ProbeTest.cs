using System;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using Product.Framework;
using Product.Tests.CommonTests;
using ProbeTests.Model;
using System.Configuration;

namespace ProbeTests.ProbeTests
{
    public abstract class ProbeTest : LoginAndConfigureTest
    {
        private const string INSTANCE_CONFIG_KEY = "Instance";
        protected override string downloadPath => _probeType+"_download/";
	    private static ProbeType _probeType;
        private static int _id;
        private static DateTime _startTime = DateTime.UtcNow;
        private string _instance;

        public ProbeTest(ProbeType probeType)
        {
            RunOnce();
            RunConfigurator.RunPath = "resources/probeRun.QA.xml";
            _probeType = probeType;
            _instance = ConfigurationManager.AppSettings.Get(INSTANCE_CONFIG_KEY);
            InitialStoring(_probeType);
            CleanUp();
        }

        private void InitialStoring(ProbeType probeType)
        {
            //using (var probesDB = new ProbesDb())
            //{
            //    var probe = new Probe
            //    {
            //        Completed = null,
            //        ProbeType = probeType,
            //        Instance = _instance.ToLower()
            //    };
            //    probesDB.Probes.Add(probe);
            //    probesDB.SaveChanges();
            //    _id = probe.ProbeId;
            //}
        }

        public abstract void Run();

        public void InsertDataToSql(DateTime end)
        {
            //using (var probesDB = new ProbesDb())
            //{
            //    var probe = probesDB.Probes.First(p => p.ProbeId == _id);
            //    probe.Completed = end;
            //    probe.IsSuccess = true;
            //    probe.ErrorText = null;

            //    probesDB.SaveChanges();
            //}
        }
        public void InsertDataToSql(DateTime end, string error)
        {
            //using (var probesDB = new ProbesDb())
            //{
            //    var probe = probesDB.Probes.First(p => p.ProbeId == _id);
            //    probe.Completed = end;
            //    probe.IsSuccess = false;
            //    probe.ErrorText = error;

            //    probesDB.SaveChanges();
            //}
            //NOTE: DEBUG
            try
            {
                LogHtml(Browser.GetDriver().PageSource);
                TakeScreenshot();
            }
            catch (Exception)
            {
                Log.Info("Screenshot saving error");
            }
        }

        public void CleanUp()
        {
            //using (var probesDB = new ProbesDb())
            //{
            //    probesDB.Database.ExecuteSqlCommand(
            //        "dbo.ArchiveProbes @ProbeType, @InstanceName",
            //        new SqlParameter("@ProbeType", _probeType),
            //        new SqlParameter("@InstanceName", _instance));
            //}
        }

	    public void TakeScreenshot()
	    {
            Screenshot screenshot = ((ITakesScreenshot)Browser.GetDriver()).GetScreenshot();
            try
            {
                screenshot.SaveAsFile(Path.GetFullPath("../Logs/" + _probeType + DateTime.UtcNow.ToString("MM_dd_HH_mm_ss") + ".png"), ScreenshotImageFormat.Png);
            }
            catch (Exception)
            {
                Log.Info("Redirecting screenshot saving");
                screenshot.SaveAsFile(Path.GetFullPath("Logs/" + _probeType + DateTime.UtcNow.ToString("MM_dd_HH_mm_ss") + ".png"), ScreenshotImageFormat.Png);
            }

        }
    }
}
