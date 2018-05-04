using BinaryTree.Power365.AutomationFramework;
using log4net;
using log4net.Config;
using System;
using System.Configuration;
using System.IO;
using System.Xml.Serialization;

namespace BinaryTree.Power365.Test
{
    public abstract class TestBase
    {
        protected AutomationContext Automation { get; set; }
        protected ILog Logger { get; set; }
        
        protected TestBase(ILog logger)
        {
            XmlConfigurator.Configure();
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            
            Logger = logger;

            var settingsFilePath = ConfigurationManager.AppSettings["SettingsFile"];
            var settings = GetSettings(settingsFilePath);

            Automation = new AutomationContext(settings);
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
