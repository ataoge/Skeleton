using System.Collections.Generic;
using JetBrains.Annotations;

namespace Ataoge.Data.Metadata
{
    public interface IViewModel
    {
        IEnumerable<UiColumnInfo> GetColumnInfos();

        UiColumnInfo CreateColumInfo(string propertyName, string jsonPropertyName = null);
        
        UiColumnInfo GetColumnInfo([NotNull]string key);

    }
}