using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepository<TEntity> : IRepository
        where TEntity : class, IEntity
    {
        #region Select/Get/Query

        IPageResult<TEntity> GetSome(Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFunc, IPageInfo pageInfo, params string [] metaData);

        IQueryable<TEntity> GetAll();

        /// <summary>
        /// 获取数据，包括指定相关联的数据
        /// </summary>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);
        
        List<TEntity> GetAllList();

        Task<List<TEntity>> GetAllListAsync();

         /// <summary>
        /// 对IQuery的结果做进一步操作，如ToList， FirstOrDefault等
        /// </summary>
        T Query<T>(Func<IQueryable<TEntity>, T> queryMethod);
        #endregion


        /// <summary>
        /// 获取单个实体， 比如：通过 Alternate Keys获得
        /// </summary>
        TEntity Single(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate);

        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);  

         #region Insert
        /// <summary>
        /// 插入一个实体
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        TEntity Insert(TEntity entity);

        /// <summary>
        /// 插入一个实体
        /// </summary>
        /// <param name="entity">要插入的实体</param>
        Task<TEntity> InsertAsync(TEntity entity);

        

        #endregion

        #region Update

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        TEntity Update(TEntity entity, Action<TEntity> updateAction = null);

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="entity">实体</param>
        Task<TEntity> UpdateAsync(TEntity entity, Func<TEntity, Task> updateAction = null);

        #endregion

        #region Delete

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">待删除的实体</param>
        void Delete(TEntity entity);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="entity">待删除的实体</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// 通过函数来删除多个实体
        /// 注意： 所有满足条件的实体都将被读取和删除。
        /// 这将导致主要的性能瓶颈，如果给定的条件会返回太多的实体。
        /// </summary>
        /// <param name="predicate">过滤实体的条件</param>
        void Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 通过函数来删除多个实体
        /// 注意： 所有满足条件的实体都将被读取和删除。
        /// 这将导致主要的性能瓶颈，如果给定的条件会返回太多的实体。
        /// </summary>
        /// <param name="predicate">过滤实体的条件</param>
        Task DeleteAsync(Expression<Func<TEntity, bool>> predicate);

        #endregion

    }
}