using BinaryTree.Power365.AutomationFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace BinaryTree.Power365DirSync.Test.Steps
{
    public class StepsBase
    {
        protected readonly FeatureContext FeatureContext;
        protected readonly ScenarioContext ScenarioContext;
        protected readonly AutomationContext AutomationContext;

        public StepsBase(FeatureContext featureContext, ScenarioContext scenarioContext, AutomationContext automationContext)
        {
            FeatureContext = featureContext;
            ScenarioContext = scenarioContext;
            AutomationContext = automationContext;
        }
    }
}
