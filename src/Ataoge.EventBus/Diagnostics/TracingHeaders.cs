using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ataoge.EventBus.Diagnostics
{
    public class TracingHeaders : IEnumerable<KeyValuePair<string, string>>
    {
        private List<KeyValuePair<string, string>> _dataStore;

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dataStore.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(string name, string value)
        {
            if (_dataStore == null)
            {
                _dataStore = new List<KeyValuePair<string, string>>();
            }

            _dataStore.Add(new KeyValuePair<string, string>(name, value));
        }

        public bool Contains(string name)
        {
            return _dataStore != null && _dataStore.Any(x => x.Key == name);
        }

        public void Remove(string name)
        {
            _dataStore?.RemoveAll(x => x.Key == name);
        }

        public void Cleaar()
        {
            _dataStore?.Clear();
        }

       
    }
}