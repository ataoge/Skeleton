using System.Collections.Generic;

namespace Ataoge.Data.Entities
{
    public abstract class CommonDataEntity<TKey> : IEntity<TKey>
    {
        //[JsonExtensionData]
        private IDictionary<string, object> innerDicts = new Dictionary<string, object>();
        
        public virtual TKey Id {get; set;}
        
        public virtual object this[string key]
        {
            get 
            {
                if (innerDicts.ContainsKey(key))
                    return innerDicts[key];
                return null;
            }
            set
            {
                innerDicts[key] = value;
            }
        }
    }
}