using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeTests.Model
{
	[Table("Probe")]
    public class Probe: ProbeBase
    {
		[Key]
		public int ProbeId { get; set; }
	}
}
