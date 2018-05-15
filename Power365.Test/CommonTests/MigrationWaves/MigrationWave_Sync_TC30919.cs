using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Net;

namespace BinaryTree.Power365.Test.CommonTests.MigrationWaves
{
    [TestClass]
    public class MigrationWave_Sync_TC30919 : TestBase
    {
        public MigrationWave_Sync_TC30919()
                   : base(LogManager.GetLogger(typeof(MigrationWave_Sync_TC30919))) { }
        private string _client;
        private string _username;
        private string _password;
        private string _project;


        [ClassInitialize]
        public static void ClassInit(TestContext testContext)
        {
            var client = Automation.Settings.GetByReference<Client>("client2");
            var project = client.GetByReference<Project>("project1");
            var sourceTenant = Automation.Settings.GetByReference<Tenant>(project.Source);
            var targetTenant = Automation.Settings.GetByReference<Tenant>(project.Target);


        }
    }
}
