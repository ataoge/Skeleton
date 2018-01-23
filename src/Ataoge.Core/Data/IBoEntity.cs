namespace Ataoge.Data
{
    public interface IExtensionByIndex
    {
        object this[string name]
        {
            get;
            set;
        }
    }

    public interface IBOEntity : IEntity, IExtensionByIndex
    {
        
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