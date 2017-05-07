using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Ataoge.Data
{
    class QueryablePageResult<TEntity> : IPageResult<TEntity>
    {
        public QueryablePageResult(IQueryable<TEntity> queryable, int count,  IEntityType builder)
        {
            _innerValue = queryable;
            _builder = builder;

            RecordCount = count;
        }

        private readonly IEntityType _builder;
        private IQueryable<TEntity> _innerValue;

        public int RecordCount {get;}

        public IEnumerator<TEntity> GetEnumerator()
        {
            return _innerValue.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IList<TMetadata> GetManyMetadata<TMetadata>(TEntity entity, string metadataName)
        {
            var nav = _builder.FindNavigation(metadataName);
            
            if (nav != null)
                return (List<TMetadata>)nav.GetGetter().GetClrValue(entity);

            return new List<TMetadata>();
        }

        public TMetadata GetOneMetadata<TMetadata>(TEntity entity, string metadataName)
        {
            var nav = _builder.FindNavigation(metadataName);
            
            if (nav != null)
                return (TMetadata)nav.GetGetter().GetClrValue(entity);
            return default(TMetadata);
        }

      
    }
}