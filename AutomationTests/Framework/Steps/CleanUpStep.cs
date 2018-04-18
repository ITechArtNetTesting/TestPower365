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
        RunConfigurator configurator ;
        Store store = new Store();

        public CleanUpStep()
        {
            configurator = new RunConfigurator(store);
        }

        public void CleanUpProjectAndTenant(String clientname)
        {
            SQLQuery queryClients = new SQLQuery(configurator.GetConnectionStringDBClients());
            String clientId = queryClients.GetClientId(clientname);
            if (clientId != null)
            {
                SQLQuery query = new SQLQuery(configurator.GetConnectionString());
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

