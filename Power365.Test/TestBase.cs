using BinaryTree.Power365.AutomationFramework;
using log4net;
using log4net.Config;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace BinaryTree.Power365.Test
{
    public abstract class TestBase
    {
        protected AutomationContext Automation
        {
            get
            {
                return _automationContext[TestContext.CurrentContext.Test.ID];
            }
        }

        protected ILog Logger
        {
            get
            {
                return _loggers[TestContext.CurrentContext.Test.Name];
            }
        }

        private ConcurrentDictionary<string, ILog> _loggers = new ConcurrentDictionary<string, ILog>();
        private ConcurrentDictionary<string, AutomationContext> _automationContext = new ConcurrentDictionary<string, AutomationContext>();

        private Settings _settings;
        
        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            XmlConfigurator.Configure();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var settingsFilePath = ConfigurationManager.AppSettings["SettingsFile"];
            _settings = GetSettings(settingsFilePath);
        }

        [SetUp]
        public void SetUp()
        {
            var logger = LogManager.GetLogger(TestContext.CurrentContext.Test.MethodName);

            if (!_loggers.TryAdd(TestContext.CurrentContext.Test.MethodName, logger))
                throw new Exception("Could not initialize Logger for test.");

            var context = new AutomationContext(_settings);
            if (!_automationContext.TryAdd(TestContext.CurrentContext.Test.ID, context))
                throw new Exception("Could not initialize AutomationContext for test.");

            Logger.InfoFormat("SetUp for Test {0} - {1}", TestContext.CurrentContext.Test.MethodName, TestContext.CurrentContext.Test.ID);
        }

        [TearDown]
        public void TearDown()
        {
            ILog logger = null;

            if (!_loggers.TryRemove(TestContext.CurrentContext.Test.MethodName, out logger))
                throw new Exception("Could not remove Logger for test.");

            AutomationContext context = null;
            if (!_automationContext.TryRemove(TestContext.CurrentContext.Test.ID, out context))
                throw new Exception("Could not remove Automation context for test.");

            context.Dispose();
        }

        private Settings GetSettings(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (FileStream settingsStream = new FileStream(path, FileMode.Open))
            {
                var settings = (Settings)serializer.Deserialize(settingsStream);
                settings.BuildReferences();
                return settings;
            }
        }
    }
}
