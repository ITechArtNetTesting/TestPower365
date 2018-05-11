using BinaryTree.Power365.AutomationFramework;
using BoDi;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using TechTalk.SpecFlow;

namespace BinaryTree.Power365DirSync.Test
{
    [Binding]
    public sealed class SetupHooks
    {
        private readonly IObjectContainer _objectContainer;
        private AutomationContext _automationContext;
        private Settings _settings;

        public SetupHooks(IObjectContainer objectContainer)
        {
            _objectContainer = objectContainer;

            var assemblyPath = System.Reflection.Assembly.GetAssembly(typeof(SetupHooks)).Location;
            var assemblyDirectory = Path.GetDirectoryName(assemblyPath);
            Directory.SetCurrentDirectory(assemblyDirectory);

            var settingsFilePath = ConfigurationManager.AppSettings["SettingsFile"];
            _settings = GetSettings(settingsFilePath);
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _automationContext = new AutomationContext(_settings);
            _objectContainer.RegisterInstanceAs(_automationContext);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            _automationContext.Dispose();
        }

        private Settings GetSettings(string path)
        {
            var serializer = new XmlSerializer(typeof(Settings), new[] { typeof(EditProjectWorkflowSettings) });
            using (FileStream settingsStream = new FileStream(path, FileMode.Open))
            {
                return (Settings)serializer.Deserialize(settingsStream);
            }
        }
    }
}
