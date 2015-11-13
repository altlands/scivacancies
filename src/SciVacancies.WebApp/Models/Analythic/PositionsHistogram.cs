using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Models.Analythic
{
    public class PositionsHistogram
    {
        public string type { get; set; }
        public string name { get; set; }
        public string toolTipContent { get; set; }
        public bool showInLegend { get; set; }
        public List<PositionDataPoint> dataPoints { get; set; } = new List<PositionDataPoint>();
    }

    public class PositionDataPoint
    {
        public long x { get; set; }
        public long y { get; set; }
        public string label { get; set; }
    }
}
