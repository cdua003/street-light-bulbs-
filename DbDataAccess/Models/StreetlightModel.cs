using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DbDataAccess
{
    public class StreetlightModel : StreetlightSummaryModel
    {
        /// <summary>
        /// Bulbs / lamps present in the light
        /// </summary>
        public IEnumerable<BulbState> Bulbs { get; set; }
        /// <summary>
        /// Is the light set to be switched on or off
        /// </summary>
        public bool IsSwitchedOn { get; set; }
        /// <summary>
        /// The draw the light takes for its electronics / gear
        /// </summary>
        public double ElectricalDraw { get; set; }
    }
}