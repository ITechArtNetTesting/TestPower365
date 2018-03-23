﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Steps;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Tests.CommonTests.SetupTests
{
    [TestClass]
    public class DatabaseCleanup 
    {
        CleanUpStep cleanUpStep;
        
        public DatabaseCleanup()
        {
            cleanUpStep = new CleanUpStep();
            RunConfigurator.RunPath = "resources/run.xml";
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        [TestCategory("Setup")]
        public void СleaningUp()
        {
            cleanUpStep = new CleanUpStep();
            cleanUpStep.CleanUpProjectAndTenant(RunConfigurator.GetRole("client1"));
            cleanUpStep.CleanUpProjectAndTenant(RunConfigurator.GetRole("client2"));


        }

    }
}