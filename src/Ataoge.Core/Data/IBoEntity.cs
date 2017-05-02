namespace Ataoge.Data
{
    public interface IBOEntity : IEntity
    {
        object this[string name]
        {
            get;
            set;
        }
    }

    public interface IBOEntity<TKey> : IEntity<TKey>
    {
        object this[string name]
        {
            get;
            set;
        }
    }
}