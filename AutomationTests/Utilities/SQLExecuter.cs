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
        public int SelectDiscoveryFrequencyHours(int clientId, string tenantName, string projectName)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionString()))
            {
                return sqlClient.SelectValue<int>($"select DiscoveryFrequencyHours from [Tenant] where clientid={Convert.ToString(clientId)} and lower(TenantName)=lower('{tenantName}') and TenantId in (SELECT TenantId FROM [ProjectTenant] INNER JOIN Project ON ProjectTenant.ProjectId=Project.ProjectId WHERE Project.ProjectName = '{projectName}')");
            }
        }

        public int SelectClientIdByName(string clientName)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionStringDBClients()))
            {
                return sqlClient.SelectValue<int>($"SELECT ClientId FROM [Client] WHERE ClientName = '{clientName}'");
            }
        }

        public int SelectProjectIdByName(string projectName)
        {
            using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionString()))
            {
                return sqlClient.SelectValue<int>($"SELECT ProjectId FROM [Project] WHERE ProjectName = '{projectName}'");
            }
        }

        //public int SelectTenantIdByProjectName(string projectName)
        //{
        //    using (var sqlClient = new SqlClient(RunConfigurator.GetConnectionString()))
        //    {
        //        return sqlClient.SelectValue<int>($"SELECT TenantId FROM [ProjectTenant] INNER JOIN Project ON ProjectTenant.ProjectId=Project.ProjectId WHERE Project.ProjectName = '{projectName}'");
        //    }
        //}
    }
}

