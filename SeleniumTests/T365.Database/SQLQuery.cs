﻿using System;
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
          DataBase.Execute("delete from Tenant where ClientId='" + clientId + "'and  TenantName='" + tenantName + "'");
        }

        public void DeleteTenant(string clientId)
        {
           DataBase.Execute("delete from Tenant where ClientId='" + clientId + "'");
        }

        public void DeleteProject(string clientId)
        {
           DataBase.Execute("delete from Project where ClientId='" + clientId + "'");
        }

        public String GetClientId(string clientName)
        {
            return DataBase.ReturnFirstExecuted("select ClientId from Client where ClientName='" + clientName + "'");
        }

    }
}