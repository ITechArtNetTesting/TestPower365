using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeMonitor.Model
{
    public partial class ProbesDB : DbContext
    {
        public ProbesDB(string connString)
            : base(connString)
        {
            Database.SetInitializer<ProbesDB>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //throw new UnintentionalCodeFirstException();
        }

        public virtual DbSet<Probe> Probes { get; set; }
        public virtual DbSet<ProbeArchive> ProbeArchives { get; set; }
    }
}
