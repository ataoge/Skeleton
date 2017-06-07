using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ataoge.Collections.Extensions;
using Ataoge.Data;
using Ataoge.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    public class EfCoreRepositoryBase<TDbContext, TEntity> : SafRepositoryBase<TEntity>, IRepositoryWithDbContext
        where TEntity : class, IEntity
        where TDbContext : DbContext, IRepositoryContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;

        /// <summary>
        /// Gets EF DbContext object.
        /// </summary>
        public virtual TDbContext Context { get { return _dbContextProvider.GetDbContext(MultiTenancySide); } }

        public override IRepositoryContext RepositoryContext => Context;

         /// <summary>
        /// Gets DbSet for given entity.
        /// </summary>
        public virtual DbSet<TEntity> Table  =>  Context.Set<TEntity>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dbContextProvider"></param>
        public EfCoreRepositoryBase(IDbContextProvider<TDbContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }

        public override IPageResult<TEntity> GetSome(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFunc, IPageInfo pageInfo, params string [] metaData)
        {
            /*IQueryable<TEntity> qq = Table; 
            if (queryFunc != null)
                qq = queryFunc(qq);
            int count = 0;
            if (pageInfo!= null && pageInfo.Index > 0)
            {
                if (pageInfo.ReturnRecordCount) 
                {
                    pageInfo.RecordCount = qq.Count();
                    count = pageInfo.RecordCount;
                }
                qq.Skip((pageInfo.Index - 1)* pageInfo.Size).Take(pageInfo.Size);
            }
            
            foreach(var metadataPath  in metaData)
            {
                qq = qq.Include(metadataPath);
            }

            return new QueryablePageResult<TEntity>(qq, count, Context.Model.FindEntityType(typeof(TEntity).FullName));*/
            var entityType = Context.Model.FindEntityType(typeof(TEntity).FullName);
            return EfCoreRepositoryHelper.GetSome(Table, entityType, queryFunc, pageInfo, metaData);
        }

        public override IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        public override IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            if (propertySelectors.IsNullOrEmpty())
            {
                return GetAll();
            }

            var query = GetAll();

            foreach (var propertySelector in propertySelectors)
            {
                query = query.Include(propertySelector);
            }

            return query;
        }

        public override async Task<List<TEntity>> GetAllListAsync()
        {
            return await GetAll().ToListAsync();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleOrDefaultAsync(predicate);
        }

        
        public override async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().FirstOrDefaultAsync(predicate);
        }

        public override TEntity Insert(TEntity entity)
        {
            return Table.Add(entity).Entity;
        }

        public override Task<TEntity> InsertAsync(TEntity entity)
        {
            return Task.FromResult(Table.Add(entity).Entity);
        }

        public override TEntity Update(TEntity entity, Action<TEntity> updateAction = null)
        {
            AttachIfNot(entity);
            if (updateAction!=null) {
                updateAction(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public override Task<TEntity> UpdateAsync(TEntity entity, Func<TEntity, Task> updateAction = null)
        {
            AttachIfNot(entity);
            if (updateAction!=null)
            {
                updateAction(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            return Task.FromResult(entity);
        }


       public override void Delete(TEntity entity)
        {
            AttachIfNot(entity);
            Table.Remove(entity);
        }

        protected virtual void AttachIfNot(TEntity entity)
        {
            var entry = Context.ChangeTracker.Entries().FirstOrDefault(ent => ent.Entity == entity);
            if (entry != null)
            {
                return;
            }
            
            Table.Attach(entity);
        }

        public DbContext GetDbContext()
        {
            return Context;
        }

    }

}