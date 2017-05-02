namespace Ataoge.Data
{
    public interface IEntity
    {

    }

    /// <summary>
    /// Represents that the implemented classes are domain entities.
    /// </summary>
    /// <typeparam name="TKey">The type of the key of the entity.</typeparam>
    public interface IEntity<TKey> : IEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the entity.
        /// </summary>
        /// <value>
        /// The identifier of the entity.
        /// </value>
        TKey Id { get; set; }
    }


    /// <summary>
    /// 树形结构实体
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface ITreeEntity<TKey> : IEntity<TKey>
    {
        TKey Pid { get; set; }
    }
}