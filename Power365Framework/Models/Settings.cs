using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Settings : ReferenceStore
    {
        public List<Database> Databases { get; set; }
        public List<Client> Clients { get; set; }
        public List<Tenant> Tenants { get; set; }

        public string BaseUrl { get; set; }
        public string DownloadsPath { get; set; }
        public string ChromeDriverPath { get; set; }
        public string O365PowerShellUrl { get; set; }
        public string MSOLConnectArgs { get; set; }
        public int TimeoutSec { get; set; }
        public string Browser { get; set; }
        public string Bitness { get; set; }
        public string HelpURL { get; set; }

        public Settings()
        {
            Databases = new List<Database>();
            Clients = new List<Client>();
            Tenants = new List<Tenant>();
        }

        public override void BuildReferences()
        {
            foreach (var db in Databases)
            {
                AddReference(db);
                db.BuildReferences();
            }

            foreach (var client in Clients)
            {
                AddReference(client);
                client.BuildReferences();
            }

            foreach (var tenant in Tenants)
            {
                AddReference(tenant);
                tenant.BuildReferences();
            }
        }
    }

}
