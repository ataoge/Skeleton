using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepositoryOfTreeEntity<TEntity, TPrimaryKey>
         where TEntity : class//, ITreeEntity<TPrimaryKey> 
    {
        IQueryable<TEntity> GetChildren(TPrimaryKey id, bool recursion = false, Expression<Func<TEntity, bool>> where = null, string orderBySilbing = null);

        List<TEntity> GetChildrenList(TPrimaryKey id, bool recursion = false, Expression<Func<TEntity, bool>> where = null, string orderBySilbing = null);

        IQueryable<TEntity> GetSiblings(TPrimaryKey id, string orderBySilbing = null);

        IQueryable<TEntity> GetAllChildren(Expression<Func<TEntity, bool>> where, string orderBySilbing = null);

        IQueryable<TEntity> GetParents(TPrimaryKey id);

        List<TEntity> GetParentList(TPrimaryKey id);
    }
}
