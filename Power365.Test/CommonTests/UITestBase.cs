using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinaryTree.Power365.AutomationFramework;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BinaryTree.Power365.Test.CommonTests
{
    public class UITestBase : TestBase
    {
        protected UITestBase(ILog logger) : base(logger)
        {
        }
        [TestCleanup]
        public void Dispose_()
        {
            Automation.Dispose();

        }
    }
}
