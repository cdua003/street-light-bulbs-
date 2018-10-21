using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Fault
    {
        public Guid BulbId { get; set; }
        public FaultCode Code { get; set; }
    }
}
