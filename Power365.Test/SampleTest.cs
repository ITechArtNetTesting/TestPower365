using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System.Threading;
using System.Reflection;
using System.Linq;

namespace BinaryTree.Power365.Test
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public class EnvironmentConfigurationAttribute : Attribute
    {

    }

    
    public class RequirementAttribute: EnvironmentConfigurationAttribute
    {
        public string ProjectType { get; set; }
        public int MailboxCount { get; set; }
        public int PublicFolderCount { get; set; }
    }

    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    [Requirement(ProjectType = "Integration")]
    public class SampleTest
    {
        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod1()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod2()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod3()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod4()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod5()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod6()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod7()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod8()
        {
            Thread.Sleep(5000);
        }

        [Test]
        [Category("Sample")]
        [Requirement(PublicFolderCount = 1)]
        public void TestMethod9()
        {
            Thread.Sleep(5000);
        }

        [OneTimeSetUp]
        public void OneTime()
        {
            var fixtureAssembly = Assembly.GetAssembly(GetType());

            var methods = fixtureAssembly.GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(EnvironmentConfigurationAttribute), false).Length > 0)
                      .ToArray();


            var attributes = methods.SelectMany(t => t.CustomAttributes).ToArray();

        }

    }
}
