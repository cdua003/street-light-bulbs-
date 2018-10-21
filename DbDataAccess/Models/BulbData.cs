using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class BulbData
    {
        public double BulbTemperature { get; set; }
        public double BulbHours { get; set; }
        public bool IsOn { get; set; }
        /// <summary>
        /// When a fault is registered displays the fault code of the bulb
        /// </summary>
        public FaultCode? FaultCondition { get; set; }
    }
}
