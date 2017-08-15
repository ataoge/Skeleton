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
    public class EfCoreRepositoryBase<TDbContext, TEntity, TPrimaryKey> : SafRepositoryBase<TEntity, TPrimaryKey>, IRepositoryWithDbContext
        where TEntity : class, IEntity<TPrimaryKey>
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

        public override IEnumerable<TEntity> GetListFromRawSQL(string sql, params object[] parameters)
        {
             return this.Context.Database.GetModelFromQuery(()=> CreateNew() , sql, parameters);
            
        }

        protected virtual TEntity CreateNew()
        {
            return Activator.CreateInstance<TEntity>();
        }

        public override async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await GetAll().SingleAsync(predicate);
        }

        public override async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = GetAll();
            if (!propertySelectors.IsNullOrEmpty())
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }
            return await query.SingleOrDefaultAsync(predicate);
        }

        public override async Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id)
        {
            return await GetAll().FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
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

        public override TPrimaryKey InsertAndGetId(TEntity entity)
        {
            entity = Insert(entity);

            if (IsTransient(entity.Id))
            {
                RepositoryContext.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity)
        {
            entity = await InsertAsync(entity);
            
            if (IsTransient(entity.Id))
            {
                await RepositoryContext.SaveChangesAsync();
            }

            return entity.Id;
        }

        public override TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            entity = InsertOrUpdate(entity);

            if (IsTransient(entity.Id))
            {
                RepositoryContext.SaveChanges();
            }

            return entity.Id;
        }

        public override async Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity)
        {
            entity = await InsertOrUpdateAsync(entity);

            if (IsTransient(entity.Id))
            {
                await RepositoryContext.SaveChangesAsync();
            }

            return entity.Id;
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

        public override void Delete(TPrimaryKey id)
        {
            var entity = GetFromChangeTrackerOrNull(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            entity = FirstOrDefault(id);
            if (entity != null)
            {
                Delete(entity);
                return;
            }

            //Could not found the entity, do nothing.
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

        private TEntity GetFromChangeTrackerOrNull(TPrimaryKey id)
        {
            var entry = Context.ChangeTracker.Entries()
                .FirstOrDefault(
                    ent =>
                        ent.Entity is TEntity &&
                        EqualityComparer<TPrimaryKey>.Default.Equals(id, (ent.Entity as TEntity).Id)
                );

            return entry?.Entity as TEntity;
        }

        public override IPageResult<TEntity> GetSome(IPageInfo pageInfo, Expression<Func<TEntity, bool>> predicate, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFunc, params string[] metaData)
        {
            throw new NotImplementedException();
        }

        
    }
}