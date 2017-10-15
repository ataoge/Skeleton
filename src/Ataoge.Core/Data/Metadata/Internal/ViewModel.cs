using System.Collections.Generic;
using Ataoge.Utilities;
using JetBrains.Annotations;

namespace Ataoge.Data.Metadata.Internal
{
    public class ViewModel : IViewModel
    {
        public ViewModel()
        {

        }

        private readonly SortedDictionary<string, UiColumnInfo> _columnInfos
            = new SortedDictionary<string, UiColumnInfo>();


        public UiColumnInfo GetColumnInfo([NotNull] string key)
        {
            var normalizekey = StringUtils.NormalizeForKey(key);
            if (_columnInfos.ContainsKey(normalizekey))
                return _columnInfos[normalizekey];

            return null;
        }

        public virtual UiColumnInfo CreateColumInfo(string propertyName, string jsonPropertyName = null)
        {
            UiColumnInfo columnInfo = null;
            if (string.IsNullOrEmpty(jsonPropertyName))
                columnInfo= new UiColumnInfo(propertyName);
            else
                columnInfo = new UiColumnInfo(propertyName, jsonPropertyName);

            if (!_columnInfos.ContainsKey(columnInfo.Key))
                _columnInfos.Add(columnInfo.Key, columnInfo);
            return columnInfo;
        }

        public IEnumerable<UiColumnInfo> GetColumnInfos() => _columnInfos.Values;
       
    }
}