namespace Ataoge.Data.Entities.Auditing
{
    public interface ICreationAudited<TKey> : IHasCreationTime
    {
        /// <summary>
        /// Id of the creator user of this entity.
        /// </summary>
        TKey Creator { get; set; }
    }
}