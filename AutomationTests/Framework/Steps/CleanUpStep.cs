using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T365.Database;
using T365Framework;

namespace Product.Framework.Steps
{
    public class CleanUpStep
    {
        public void CleanUpProjectAndTenant(String clientname)
        {
            SQLQuery queryClients = new SQLQuery(RunConfigurator.GetConnectionStringDBClients());
            String clientId = queryClients.GetClientId(clientname);
            if (clientId != null)
            {
                SQLQuery query = new SQLQuery(RunConfigurator.GetConnectionString());
                query.DeleteProject(clientId);
                query.DeleteTenant(clientId);
            }
            else
            {
                Console.WriteLine($"Client with id {clientId} can not be found");
            }

        }
    }

}

