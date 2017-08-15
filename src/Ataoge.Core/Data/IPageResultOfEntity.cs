using System.Collections.Generic;

namespace Ataoge.Data
{
    public interface IPageResult<TEntity> : IEnumerable<TEntity>
    {
        TMetadata GetOneMetadata<TMetadata>(TEntity entity, string metadataName);
        IList<TMetadata> GetManyMetadata<TMetadata>(TEntity entity, string metadataName);
        int RecordCount {get; }
        int? FilteredRecord {get;}
    }
}