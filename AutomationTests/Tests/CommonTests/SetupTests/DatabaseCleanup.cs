using Microsoft.VisualStudio.TestTools.UnitTesting;
using Product.Framework;
using Product.Framework.Steps;
using Product.Utilities;
using System;
using System.IO;

namespace Product.Tests.CommonTests.SetupTests
{
    [TestClass]
    public class DatabaseCleanup 
    {
        public DatabaseCleanup()
        {
            RunConfigurator.RunPath = "resources/run.xml";
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        [TestMethod]
        [TestCategory("Setup")]
        public void CleaningUp()
        {
            CleanProjectAndTenant(RunConfigurator.GetClient("client1"));
            CleanProjectAndTenant(RunConfigurator.GetClient("client2"));
        }

        private void CleanProjectAndTenant(string client)
        {
            int clientId = 0;
            using (var dbClients = new SqlClient(RunConfigurator.GetConnectionStringDBClients()))
            {
                clientId = dbClients.SelectValue<int>(string.Format("SELECT ClientId FROM [Client] WHERE ClientName = '{0}'", client));
            }

            if (clientId <= 0)
                throw new Exception(string.Format("Could not locate Client '{0}'", client));

            using (var dbTenant2Tenant = new SqlClient(RunConfigurator.GetConnectionString()))
            {
                dbTenant2Tenant.ExecuteNonQuery(string.Format("DELETE FROM [Project] WHERE ClientId = {0}", clientId));
                dbTenant2Tenant.ExecuteNonQuery(string.Format("DELETE FROM [Tenant] WHERE ClientId = {0}", clientId));
            }
        }

    }
}
