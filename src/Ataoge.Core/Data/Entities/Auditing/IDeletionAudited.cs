namespace Ataoge.Data.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities which wanted to store deletion information (who and when deleted).
    /// </summary>
    public interface IDeletionAudited<TKey> : IHasDeletionTime
        where TKey : struct
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        TKey? Deleter { get; set; }
    }

    public interface IDeletionAuditedString : IHasDeletionTime
    {
        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        string Deleter { get; set; }
    }
}