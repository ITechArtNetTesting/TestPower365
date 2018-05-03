using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using TP365Framework.Drivers;
using TP365Framework.StaticClasses;
using TP365Framework.Steps;

namespace Product.Tests.TP365TestCases.Abstractions
{
    [TestClass]
    public abstract class TestCase
    {
        public Driver driver;
        
        [TestInitialize]
        public void BeforeAllTests()
        {
            RunConfigurator.RunPath = "resources/run.xml";
            driver = new Driver(WebBrowsers.Chrome, RunConfigurator.GetValue("baseurl"));
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
