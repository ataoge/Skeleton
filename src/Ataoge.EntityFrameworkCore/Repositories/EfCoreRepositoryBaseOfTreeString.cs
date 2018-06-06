using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Ataoge.Data;
using Ataoge.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    public class EfCoreTreeRepositoryBase<TDbContext, TEntity> : EfCoreRepositoryBase<TDbContext, TEntity, string>, IRepositoryOfTreeEntity<TEntity, string>
        where TEntity : class, ITreeEntity
        where TDbContext : DbContext, IRepositoryContext
    {
        public EfCoreTreeRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider) : base(dbContextProvider)
        {
            _repositoryHelper = base.Context as IRepositoryHelper;
        }

        private IRepositoryHelper _repositoryHelper;

        public List<TEntity> GetChildrenList(string id, bool recursion = false, Expression<Func<TEntity, bool>> where = null, string orderBySilbing = null)
        {
            return GetChildren(id, recursion, where, orderBySilbing).ToList();
        }

        public IQueryable<TEntity> GetChildren(string id, bool recursion = false, Expression<Func<TEntity, bool>> where = null, string orderBySilbing = null)
        {
            if (recursion)
            {
                var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
                return EfCoreRepositoryHelper.TreeQuery<TEntity, string>(_repositoryHelper.ProviderName, Table, entityType, ts => ts.Id.Equals(id), where, false, orderBySilbing);
            }
            
            IQueryable<TEntity> result = Table;
            if (where != null)
                result = result.Where(where);
            return result.Where(t => t.Pid ==id);
           
        }

        public IQueryable<TEntity> GetSiblings(string id, string orderBySilbing = null)
        {
            TEntity theEntity = Get(id); 
            return Table.Where(t=> t.Pid == theEntity.Pid);
            // 构造表达式；
        }

        public IQueryable<TEntity> GetAllChildren(Expression<Func<TEntity, bool>> where, bool startQuery = false, string orderBySilbing = null)
        {
            
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
            if (startQuery)
                return  EfCoreRepositoryHelper.TreeQuery<TEntity,string>(_repositoryHelper.ProviderName, Table, entityType, where, null, false, orderBySilbing, 0, distinct:true);
            return  EfCoreRepositoryHelper.TreeQuery<TEntity,string>(_repositoryHelper.ProviderName, Table, entityType, t => t.Pid == null, where, false);
        }

        public List<TEntity> GetParentList(string id)
        {
            return GetParents(id).ToList();
        }

        public IQueryable<TEntity> GetParents(string id)
        {
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
            if (_repositoryHelper != null)
                return EfCoreRepositoryHelper.TreeQuery<TEntity,string>(_repositoryHelper, Table, entityType, t => t.Id.Equals(id), null, true);
            return EfCoreRepositoryHelper.TreeQuery<TEntity,string>(_repositoryHelper.ProviderName, Table, entityType, t => t.Id.Equals(id), null, true);
        }

        public IQueryable<TResult> GetParents<TResult>(Expression<Func<TEntity, bool>> startQuery, Expression<Func<TEntity, TResult>> selector)
            where TResult : class
         {
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);

             return EfCoreRepositoryHelper.TreeQuery<TEntity,string, TResult>(_repositoryHelper, Table, entityType, startQuery, selector,  null, true, null, 0, true);
        }

         public IEnumerable<string> GetParents(Expression<Func<TEntity, bool>> startQuery)
         {
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);

             return EfCoreRepositoryHelper.TreeQuery<TEntity, string>(_repositoryHelper, Table, entityType, startQuery, null, true, null, 0, true).Select(t => t.Id);
        }

        public IEnumerable<string> GetChildrenRecursion(Expression<Func<TEntity, bool>> startQuery)
        {
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
            return EfCoreRepositoryHelper.TreeQuery<TEntity, string>(_repositoryHelper, Table, entityType, startQuery, null, false, null, 0, true).Select(t => t.Id);
        }

       

        public IQueryable<TResult> GetChildrenRecursion<TResult>(Expression<Func<TEntity, bool>> startQuery, Expression<Func<TEntity, TResult>> selector)
            where TResult : class
        {
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
            return EfCoreRepositoryHelper.TreeQuery<TEntity,string, TResult>(_repositoryHelper, Table, entityType, startQuery, selector,  null, false, null, 0, true);
        }
         
    }
    
}