using System;
using System.Collections.Generic;
using System.Linq;
using Ataoge.EventBus.Monitoring;
using Ataoge.EventBus.Utilities;

namespace Ataoge.EventBus.EfCore
{
    internal class EfCoreMonitoringApi : IMonitoringApi
    {
        private readonly EfCoreOptions _options;
        private readonly EfCoreStorage _storage;

        public EfCoreMonitoringApi(EfCoreOptions options, IStorage storage)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _storage = storage as EfCoreStorage ?? throw new ArgumentNullException(nameof(storage)); ;
        }

        public StatisticsDto GetStatistics()
        {
            var statistics = _storage.UseDbContext(dbContext =>
            {
                var stats = new StatisticsDto();
                stats.PublishedSucceeded = dbContext.Set<Models.PublishedMessage>().Where(t => t.StatusName == StatusName.Succeeded).Count();
                stats.PublishedFailed = dbContext.Set<Models.PublishedMessage>().Where(t => t.StatusName == StatusName.Failed).Count();
                stats.ReceivedSucceeded = dbContext.Set<Models.ReceivedMessage>().Where(t => t.StatusName == StatusName.Succeeded).Count();
                stats.ReceivedFailed = dbContext.Set<Models.ReceivedMessage>().Where(t => t.StatusName == StatusName.Failed).Count();
                return stats;
            });
            return statistics;
        }

        

        public IList<MessageDto> Messages(MessageQueryDto queryDto)
        {
            if (queryDto.MessageType == MessageType.Publish)
            {
                return _storage.UseDbContext(dbContext =>
                {
                    IQueryable<Models.PublishedMessage> queryable = dbContext.Set<Models.PublishedMessage>();
                    if (!string.IsNullOrEmpty(queryDto.StatusName))
                    {
                        queryable = queryable.Where(t => string.Equals(t.StatusName, queryDto.StatusName, StringComparison.InvariantCultureIgnoreCase));

                    }
                    if (!string.IsNullOrEmpty(queryDto.Name))
                    {
                        queryable = queryable.Where(t => t.Name == queryDto.Name);

                    }
                    if (!string.IsNullOrEmpty(queryDto.Content))
                    {
                        queryable = queryable.Where(t => t.Name == queryDto.Content);

                    }
                    return queryable.OrderBy(t => t.Added).Skip(queryDto.CurrentPage * queryDto.PageSize).Take(queryDto.PageSize).Select(t => new MessageDto()
                    {
                        Name = t.Name,
                        Content = t.Content,
                        Added = t.Added,
                        StatusName = t.StatusName,
                        Retries = t.Retries,
                        Id = t.Id,
                        ExpiresAt = t.ExpiresAt,
                        Version = _options.Version
                        // Version = dbContext.Entry(t).Property("Version").CurrentValue?.ToString()

                    }).ToList();
                });
            }
            else
            {

                return _storage.UseDbContext(dbContext =>
                {
                    IQueryable<Models.ReceivedMessage> queryable = dbContext.Set<Models.ReceivedMessage>();
                    if (!string.IsNullOrEmpty(queryDto.StatusName))
                    {
                        queryable = queryable.Where(t => string.Equals(t.StatusName, queryDto.StatusName, StringComparison.InvariantCultureIgnoreCase));

                    }
                    if (!string.IsNullOrEmpty(queryDto.Name))
                    {
                        queryable = queryable.Where(t => t.Name == queryDto.Name);

                    }
                    if (!string.IsNullOrEmpty(queryDto.Group))
                    {
                        queryable = queryable.Where(t => t.Group == queryDto.Group);

                    }
                    if (!string.IsNullOrEmpty(queryDto.Content))
                    {
                        queryable = queryable.Where(t => t.Name == queryDto.Content);

                    }
                    return queryable.OrderBy(t => t.Added).Skip(queryDto.CurrentPage * queryDto.PageSize).Take(queryDto.PageSize).Select(t => new MessageDto()
                    {
                        Name = t.Name,
                        Group = t.Group,
                        Content = t.Content,
                        Added = t.Added,
                        StatusName = t.StatusName,
                        Retries = t.Retries,
                        Id = t.Id,
                        ExpiresAt = t.ExpiresAt,
                        Version = _options.Version
                        // Version = dbContext.Entry(t).Property("Version").CurrentValue?.ToString()

                    }).ToList();
                });
            }
         
        }

        public int PublishedFailedCount()
        {
            return GetNumberOfPublishedMessage(StatusName.Failed);
        }

        public int PublishedSucceededCount()
        {
            return GetNumberOfPublishedMessage(StatusName.Failed);
        }

        public int ReceivedFailedCount()
        {
            return GetNumberOfReceivedMessage(StatusName.Failed);
        }

        public int ReceivedSucceededCount()
        {
            return GetNumberOfReceivedMessage(StatusName.Succeeded);
        }

        private int GetNumberOfPublishedMessage(string statusName)
        {
            return _storage.UseDbContext<Models.PublishedMessage, int>(queryable => {
                return queryable.Where(t => t.StatusName == statusName).Count();
            });
        }

        private int GetNumberOfReceivedMessage(string statusName)
        {
            return _storage.UseDbContext<Models.ReceivedMessage, int>(queryable => {
                return queryable.Where(t => t.StatusName == statusName).Count();
            });
        }

        public IDictionary<DateTime, int> HourlyFailedJobs(MessageType type)
        {
            return GetHourlyTimelineStats(type, StatusName.Failed);
        }

        public IDictionary<DateTime, int> HourlySucceededJobs(MessageType type)
        {
            return GetHourlyTimelineStats(type, StatusName.Succeeded);
        }

         private Dictionary<DateTime, int> GetHourlyTimelineStats(MessageType type,
            string statusName)
        {
            var endDate = DateTime.Now;
            var dates = new List<DateTime>();
            for (var i = 0; i < 24; i++)
            {
                dates.Add(endDate);
                endDate = endDate.AddHours(-1);
            }

            var keyMaps = dates.ToDictionary(x => x.ToString("yyyy-MM-dd-HH"), x => x);

            return GetTimelineStats(type, statusName, keyMaps);
        }

        private Dictionary<DateTime, int> GetTimelineStats(
            MessageType type,
            string statusName,
            IDictionary<string, DateTime> keyMaps)
        {
            IDictionary<string,int> valuesMap = null;
            if (type == MessageType.Publish) 
            {
                valuesMap = _storage.UseDbContext<Models.PublishedMessage, IDictionary<string,int>>(queryable => {
                    queryable = queryable.Where(t => t.StatusName == statusName);
                    var aa = queryable.GroupBy(t => t.Added.ToString("yyyy-MM-dd-HH")).Select(g => new { Key = g.Key, Count = g.Count()});
                    return aa.ToDictionary(x => x.Key, x => x.Count);
                });
            }
            else
            {
                valuesMap = _storage.UseDbContext<Models.ReceivedMessage, IDictionary<string,int>>(queryable => {
                    queryable = queryable.Where(t => t.StatusName == statusName);
                    var aa = queryable.GroupBy(t => t.Added.ToString("yyyy-MM-dd-HH")).Select(g => new { Key = g.Key, Count = g.Count()});
                    return aa.ToDictionary(x => x.Key, x => x.Count);
                });
            }

            foreach (var key in keyMaps.Keys)
            {
                if (!valuesMap.ContainsKey(key))
                {
                    valuesMap.Add(key, 0);
                }
            }

            var result = new Dictionary<DateTime, int>();
            for (var i = 0; i < keyMaps.Count; i++)
            {
                var value = valuesMap[keyMaps.ElementAt(i).Key];
                result.Add(keyMaps.ElementAt(i).Value, value);
            }

            return result;
        }
    }
}