using System;
using System.Linq;
using ITechArtTestFramework.Drivers;
using ITechArtTestFramework.StaticClasses;
using ITechArtTestFramework.Steps;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ITechArtTestFramework.TestCases
{
    [TestClass]
    public abstract class TestCase
    {        
        public Driver driver = new Driver(WebBrowsers.Chrome);

        //Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        //    RunConfigurator.RunPath = "resources/run.xml";
        //    RunConfigurator.DownloadPath = downloadPath;
        //    RunConfigurator.ResourcesPath = "resources/";

        [TestInitialize]
        public void BeforeAllTests()
        {                    
            InitSteps();
        }

        [TestCleanup]
        public void AfterAllTests()
        {
            driver.CloseDriver();
        }

        private void InitSteps()
        {           
            var testCaseFields = GetType().GetFields(FrameworkConstants.BindingFlags).Where(field=>field.FieldType.IsSubclassOf(FrameworkConstants.BaseStepsType));
            foreach (var field in testCaseFields)
            {
                field.SetValue(this, Activator.CreateInstance(field.FieldType));
                ((BaseSteps)field.GetValue(this)).InitPageObjects(driver);                
            }
        }
    }
}
