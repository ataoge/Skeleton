using System.Collections.Generic;

namespace Ataoge.Services
{
    public interface ICachedItemManager<TItem>
        where TItem : class
    {
        bool CacheItems();

        bool RecacheItems();

        TItem GetItem(string name, bool onlyInCache = true);

        IEnumerable<TItem> GetItems();
    }
}