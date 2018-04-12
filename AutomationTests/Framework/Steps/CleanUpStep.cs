using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Database;

namespace Product.Framework.Steps
{
    public class CleanUpStep:BaseEntity
    {
        public void CleanUpProjectAndTenant(String clientname)
        {
            SQLQuery queryClients = new SQLQuery(RunConfigurator.GetConnectionStringDBClients());
            String clientId = queryClients.GetClientId(clientname);
            if (clientId != null)
            {
                SQLQuery query = new SQLQuery(RunConfigurator.GetConnectionString());
                try
                {
                    query.DeleteProject(clientId);
                    query.DeleteTenant(clientId);
                }
                catch (Exception ex)
                {
                    throw ;
                }
            }
            else
            {
                Log.Info($"Client with id {clientId} can not be found");
            }

        }


    }

}

