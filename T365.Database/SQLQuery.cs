using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T365.Database
{
    public class SQLQuery
    {
        DB DataBase;
        public SQLQuery(string connectionString)
        {
            DataBase = new DB(connectionString);
        }
        public void DeleteTenant(string clientId, string tenantName)
        {
            try
            {
                DataBase.Execute("delete from Tenant where ClientId='" + clientId + "'and  TenantName='" + tenantName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteTenant(string clientId)
        {
            try
            {
                DataBase.Execute("delete from Tenant where ClientId='" + clientId + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DeleteProject(string clientId)
        {
            try
            {
                DataBase.Execute("delete from Project where ClientId='" + clientId + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public String GetClientId(string clientName)
        {
            try
            {
                return DataBase.ReturnFirstExecuted("select ClientId from Client where ClientName='" + clientName + "'");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
