using System;

namespace Ataoge.Data.Entities.Auditing
{
    /// <summary>
    /// This interface is implemented by entities that is wanted to store modification information (who and when modified lastly).
    /// Properties are automatically set when updating the <see cref="IEntity"/>.
    /// </summary>
    public interface IModificationAudited<TKey> : IHasModificationTime
        where TKey : struct
    {
        /// <summary>
        /// Last modifier user for this entity.
        /// </summary>
        TKey? Modifier { get; set; }
    }

    public interface IModificationAuditedString : IHasModificationTime
    {
        string Modifier { get; set; }
    }
}