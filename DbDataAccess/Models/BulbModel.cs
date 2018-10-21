using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbDataAccess
{
    public class BulbModel
    {

        public Guid Id { get; set; }
        /// <summary>
        /// The maximum temperature a bulb can sustain till auto-shutdown in Celcius
        /// </summary>
        public double MaxTemperature { get; set; }
        /// <summary>
        /// The power draw in Watts
        /// </summary>
        public double PowerDraw { get; set; }
        /// <summary>
        /// The maximum number of hours until replacement is recommended
        /// </summary>
        public int MaxHours { get; set; }
        /// <summary>
        /// The type of bulb that this is
        /// </summary>
        public string BulbType { get; set; } = "Unknown";
    }
}