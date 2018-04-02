using System;
using T365.Database;
using T365.Framework;

namespace Product.Framework.Steps
{
    public class CleanUpStep : BaseEntity
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
                catch (Exception exception)
                { throw exception; }
            }
            else
            {                
                Log.Info($"Client with id {clientId} can not be found");
            }
        }
    }
}

