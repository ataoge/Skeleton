namespace Ataoge.Data
{
    public class KeyValueClass<TKey, TValue>
    {
        public virtual TKey Key {get; set;}

        public virtual TValue Value {get; set;}
    }
}