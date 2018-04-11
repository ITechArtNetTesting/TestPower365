using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeMonitor.Model
{
    [Table("ProbeArchive")]
    public partial class ProbeArchive
    {
        [Key]
        public int ProbeId { get; set; }
        public string ProbeType { get; set; }
        public DateTime Started { get; set; }
        public DateTime? Completed { get; set; }
        public bool? IsSuccess { get; set; }
        public string ErrorText { get; set; }
    }
}
