using System.Collections.Generic;
using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepositoryOfTreeEntity<TEntity, TPrimaryKey>
         where TEntity :class, ITreeEntity<TPrimaryKey> 
    {
        List<TEntity> GetParents(TPrimaryKey id);

        List<TEntity> GetChildren(TPrimaryKey id, bool recursion = false);
    }
}