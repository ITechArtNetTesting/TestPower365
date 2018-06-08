using System;

namespace BinaryTree.Power365.AutomationFramework
{
    [Serializable]
    public class Database : Referential
    {
        public string InitialCatalog { get; set; }
        public Credential Credential { get; set; }
        public string Server { get; set; }
        
        private readonly string _azureSqlConnectionStringFormat = "Server=tcp:{0},1433; Initial Catalog = {1}; Persist Security Info = False; User ID = {2}; Password ={3}; MultipleActiveResultSets = False; Encrypt = True; TrustServerCertificate = False; Connection Timeout = 30;";
        private readonly string _localSqlConnectionStringFormat = "Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3}";


        public override void BuildReferences()
        {
            if (Credential != null)
            {
                AddReference(Credential);
                Credential.BuildReferences();
            }
        }

        public string GetAzureSqlConnectionString()
        {                    
            return string.Format(_azureSqlConnectionStringFormat, Server, InitialCatalog, Credential.Username, Credential.Password);
        }

        public string GetLocalSqlConnectionString()
        {
            return string.Format(_localSqlConnectionStringFormat, Server, InitialCatalog, Credential.Username, Credential.Password);
        }
    }
}