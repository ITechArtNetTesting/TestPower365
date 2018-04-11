using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProbeTests.Model
{
    public abstract class ProbeBase
    {
        public ProbeType ProbeType { get; set; }
        public DateTime Started
        {
            get { return started.HasValue ? started.Value : DateTime.UtcNow; }
            set { started = value; }
        }
        public DateTime? Completed { get; set; }
        public bool? IsSuccess { get; set; }
        public string ErrorText { get; set; }

        private DateTime? started = null;

        [MaxLength(50)]
        public string Instance { get; set; }
    }
}
