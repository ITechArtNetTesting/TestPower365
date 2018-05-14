using Product.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.Utilities
{
    public class SQLExecuter
    {
        public int SelectDiscoveryFrequencyHours(int clientId, string tenantName, int tenantId)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionString()))
            {
                return sqlClient.SelectValue<int>($"select DiscoveryFrequencyHours from [Tenant] where clientid={Convert.ToString(clientId)} and lower(TenantName)=lower('{tenantName}') and TenantId={Convert.ToString(tenantId)}");
            }
        }

        public int SelectClientIdByName(string clientName)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionStringDBClients()))
            {
                return sqlClient.SelectValue<int>($"SELECT ClientId FROM [Client] WHERE ClientName = '{clientName}'");
            }
        }

        public int SelectTenantIdByName(string projectName)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionString())) 
            {
                return sqlClient.SelectValue<int>($"SELECT TenantId FROM projectTenant where ProjectId in (SELECT ProjectId FROM [Project] WHERE ProjectName = '{projectName}')");
            }
        }

    }
}

