using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangamentSystem.Core.Models.Business
{
    public class HealthMetric
    {
        public int HealthMetricId { get; set; }
        public string MetricType { get; set; }
        public decimal Value { get; set; }
        public DateTime MeasurementDate { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }

}
