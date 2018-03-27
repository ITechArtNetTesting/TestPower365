using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestFramework.IoC;
using TestFramework.PageObjects.Interfaces;
using TestFramework.Steps.Interfaces;

namespace TestFramework.Steps
{
    public class RightBarSteps : IRightBarSteps
    {
        IRightBar rightBar = DependencyResolver.For<IRightBar>();
        public void ChooseClientByKeys(string keys)
        {
            rightBar.ClickClientDropDown();
            rightBar.ChooseClient(keys);
        }
    }
}
