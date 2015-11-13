using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SciVacancies.WebApp.Models.Analythic
{
    public class PaymentsHistogram
    {
        public string type { get; set; }
        public string axisYType { get; set; }
        public string name { get; set; }
        public bool showInLegend { get; set; }
        public List<PaymentDataPoint> dataPoints { get; set; } = new List<PaymentDataPoint>();
    }

    public class PaymentDataPoint
    {
        public long x { get; set; }
        public double y { get; set; }
        public string label { get; set; }
    }
}
