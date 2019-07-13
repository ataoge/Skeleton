using System;
using System.Collections.Generic;

namespace Ataoge.EventBus.Dashboard
{
    public static class NavigationMenu
    {
        public static readonly List<Func<string, MenuItem>> Items = new List<Func<string, MenuItem>>();

        static NavigationMenu()
        {
            Items.Add(page => new MenuItem("published", "/published")
            {
                Active = false,//page.RequestPath.StartsWith("/published"),
                Metrics = null//new[]
                //{
                    //DashboardMetrics.PublishedSucceededCount,
                    //DashboardMetrics.PublishedFailedCountOrNull
                //}
            });

            
        }
    }
}