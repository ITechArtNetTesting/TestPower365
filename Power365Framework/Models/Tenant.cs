using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Tenant : Referential
    {
        public List<Credential> Credentials { get; set; }
        public string Name { get; set; }
        public string PrimaryDomain { get; set; }
        public string SecondaryDomain { get; set; }
        public string AzureADSyncServer { get; set; }
        public string ExchangePowerShellUrl { get; set; }

        public Tenant()
        {
            Credentials = new List<Credential>();
        }

        public override void BuildReferences()
        {
            foreach (var credential in Credentials)
            {
                AddReference(credential);
                credential.BuildReferences();
            }
        }

    }
}