using System;

namespace Ataoge.Data.Entities.Auditing
{
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? LastModifyTime {get; set;}
    }
}