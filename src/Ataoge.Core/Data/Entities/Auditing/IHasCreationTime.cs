using System;

namespace Ataoge.Data.Entities.Auditing
{
    public interface IHasCreationTime
    {
        /// <summary>
        /// Creation time of this entity.
        /// </summary>
        DateTime CreateTime {get; set;}
    }
}