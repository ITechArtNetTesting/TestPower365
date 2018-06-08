using AutomationServices.PowerShell;
using AutomationServices.SqlDatabase;
using BinaryTree.Power365.AutomationFramework;
using BinaryTree.Power365.Test.Services;
using log4net;
using log4net.Config;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.IO;
using System.Reflection;
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

        private RemoteCacheService _remoteCacheService;

        private ILog _baseLogger = LogManager.GetLogger(typeof(TestBase));

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _baseLogger.InfoFormat("OneTimeSetUp");

            XmlConfigurator.Configure();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            var settingsFilePath = ConfigurationManager.AppSettings["SettingsFile"];
            _settings = GetSettings(settingsFilePath);

            _remoteCacheService = new RemoteCacheService();
        }

        [SetUp]
        public void SetUp()
        {
            var logger = LogManager.GetLogger(TestContext.CurrentContext.Test.MethodName);
            logger.InfoFormat("SetUp for Test {0} - {1}", TestContext.CurrentContext.Test.MethodName, TestContext.CurrentContext.Test.ID);

            if (!_loggers.TryAdd(TestContext.CurrentContext.Test.MethodName, logger))
                throw new Exception("Could not initialize Logger for test.");

            var context = new AutomationContext(_settings, TestContext.CurrentContext.Test.MethodName);
            ConfigureServices(context);

            if (!_automationContext.TryAdd(TestContext.CurrentContext.Test.ID, context))
                throw new Exception("Could not initialize AutomationContext for test.");
            
            LockResources();
        }

        [TearDown]
        public void TearDown()
        {
            UnlockResources();
            
            Logger.InfoFormat("TearDown for Test {0} - {1}", TestContext.CurrentContext.Test.MethodName, TestContext.CurrentContext.Test.ID);
            Logger.InfoFormat("[TestResult] => [{0}] <= {1}", TestContext.CurrentContext.Result.Outcome.Status, TestContext.CurrentContext.Test.FullName);
            
            switch (TestContext.CurrentContext.Result.Outcome.Status)
            {
                case NUnit.Framework.Interfaces.TestStatus.Inconclusive:
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Skipped:
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Passed:
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Warning:
                    break;
                case NUnit.Framework.Interfaces.TestStatus.Failed:
                    Logger.Error(string.Format("[Failure Reason] => {0}", TestContext.CurrentContext.Result.Message));
                    Logger.Debug(string.Format("[Stack Trace] => {0}", TestContext.CurrentContext.Result.StackTrace));
                    break;
                default:
                    break;
            }

            ILog logger = null;
            if (!_loggers.TryRemove(TestContext.CurrentContext.Test.MethodName, out logger))
                throw new Exception("Could not remove Logger for test.");

            AutomationContext context = null;
            if (!_automationContext.TryRemove(TestContext.CurrentContext.Test.ID, out context))
                throw new Exception("Could not remove Automation context for test.");
            
            if(context != null)
                context.Dispose();
        }

        protected virtual void ConfigureServices(AutomationContext context)
        {
            context.RegisterService(new PowerShellService());
            context.RegisterService(new DatabaseService(_settings));
        }

        private void LockResources()
        {
            var testMethod = GetType().GetMethod(TestContext.CurrentContext.Test.MethodName);
            var attributes = testMethod.GetCustomAttributes<TestResourceAttribute>();

            foreach (var attr in attributes)
            {
                ThrowIfResourceInUse(attr);
                SetResourceInUse(attr);
            }
        }

        private void UnlockResources()
        {
            var testMethod = GetType().GetMethod(TestContext.CurrentContext.Test.MethodName);
            var attributes = testMethod.GetCustomAttributes<TestResourceAttribute>();

            foreach (var attr in attributes)
            {
                SetResourceFree(attr);
            }
        }

        private void ThrowIfResourceInUse(TestResourceAttribute resourceAttribute)
        {
            var client = _settings.GetByReference<Client>(resourceAttribute.Client);
            var project = client.GetByReference<Project>(resourceAttribute.Project);
            var entry = project.GetByReference<UserMigration>(resourceAttribute.Entry);

            if (_remoteCacheService.IsResourceInUse(client.Name, project.Name, entry.Source))
                throw new Exception("Resource already in use");
        }

        private void SetResourceInUse(TestResourceAttribute resourceAttribute)
        {
            var client = _settings.GetByReference<Client>(resourceAttribute.Client);
            var project = client.GetByReference<Project>(resourceAttribute.Project);
            var entry = project.GetByReference<UserMigration>(resourceAttribute.Entry);

            _baseLogger.Info(string.Format("SetResourceInUse: {0} - {1} - {2}", client.Name, project.Name, entry.Source));
            _remoteCacheService.SetResourceInUse(client.Name, project.Name, entry.Source);
        }

        private void SetResourceFree(TestResourceAttribute resourceAttribute)
        {
            var client = _settings.GetByReference<Client>(resourceAttribute.Client);
            var project = client.GetByReference<Project>(resourceAttribute.Project);
            var entry = project.GetByReference<UserMigration>(resourceAttribute.Entry);

            _baseLogger.Info(string.Format("SetResourceFree: {0} - {1} - {2}", client.Name, project.Name, entry.Source));
            _remoteCacheService.SetResourceFree(client.Name, project.Name, entry.Source);
        }

        private Settings GetSettings(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings));
            using (FileStream settingsStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                var settings = (Settings)serializer.Deserialize(settingsStream);
                settings.BuildReferences();
                return settings;
            }
        }
    }
}
