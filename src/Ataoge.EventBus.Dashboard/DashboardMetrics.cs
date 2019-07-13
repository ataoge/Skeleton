using System;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.EventBus.Dashboard
{
    public static class DashboardMetrics
    {
        private static readonly Dictionary<string, DashboardMetric> Metrics = new Dictionary<string, DashboardMetric>();

        static DashboardMetrics()
        {
        }

        public static void AddMetric(DashboardMetric metric)
        {
            if (metric == null)
            {
                throw new ArgumentNullException(nameof(metric));
            }

            lock (Metrics)
            {
                Metrics[metric.Name] = metric;
            }
        }

        //public static readonly DashboardMetric PublishedSucceededCount = new DashboardMetric(
        //    "published_succeeded:count",
        //    "Metrics_SucceededJobs",
            //page => new Metric(page.Statistics.PublishedSucceeded.ToString("N0"))
            //{
            //    IntValue = page.Statistics.PublishedSucceeded
            //});

        public static IEnumerable<DashboardMetric> GetMetrics()
        {
            lock (Metrics)
            {
                return Metrics.Values.ToList();
            }
        }
    }
}