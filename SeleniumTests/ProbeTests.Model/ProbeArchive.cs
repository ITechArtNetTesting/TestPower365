using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeTests.Model
{
	[Table("ProbeArchive")]
	public class ProbeArchive: ProbeBase
    {
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int ProbeId { get; set; }
	}
}
