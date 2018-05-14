//using BTCloud.Lib;
using System.Data.Entity;

namespace ProbeTests.Model
{
    public class ProbesDb: DbContext
    {
        //public ProbesDb():
        //    base(Encryption.ConnectionString("ProbesDB"))
        //{
        //    Database.SetInitializer<ProbesDb>(null);
        //}
        public ProbesDb(string connectionString) :
            base(connectionString)
        {
            Database.SetInitializer<ProbesDb>(null);
        }

        public DbSet<Probe> Probes { get; set; }
        public DbSet<ProbeArchive> ProbeArchives { get; set; }
    }
}
