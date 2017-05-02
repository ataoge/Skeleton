using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepository<TEntity, TPrimaryKey> : IRepository<TEntity>
        where TEntity :class, IEntity<TPrimaryKey>
    {
       

        #region 插入 Get

        TEntity Get(TPrimaryKey id);

        /// <summary>
        /// </summary>
        Task<TEntity> GetAsync(TPrimaryKey id);

        
        TEntity FirstOrDefault(TPrimaryKey id);

        Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id);

        #endregion;

        #region Insert
        

        /// <summary>
        /// 插入一个实体并返回它的主键。
        /// 可能需要保存工作单元，才能得到返回的主键ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的主键</returns>
        TPrimaryKey InsertAndGetId(TEntity entity);

        /// <summary>
        /// 插入一个实体并返回它的主键。
        /// 可能需要保存工作单元，才能得到返回的主键ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的主键</returns>
        Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity);

        /// <summary>
        /// 插入或者更新实体，取决于主键是否包含有意义的值
        /// </summary>
        /// <param name="entity">实体</param>
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// 插入或者更新实体，取决于主键是否包含有意义的值
        /// </summary>
        /// <param name="entity">实体</param>
        Task<TEntity> InsertOrUpdateAsync(TEntity entity);

        /// <summary>
        /// 取决于主键是否包含有意义的值来插入或者更新一个实体，并返回它的主键。
        /// 可能需要保存工作单元，才能得到返回的主键ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的主键</returns>
        TPrimaryKey InsertOrUpdateAndGetId(TEntity entity);

        /// <summary>
        /// 取决于主键是否包含有意义的值来插入或者更新一个实体，并返回它的主键。
        /// 可能需要保存工作单元，才能得到返回的主键ID
        /// </summary>
        /// <param name="entity">实体</param>
        /// <returns>实体的主键</returns>
        Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity);

        #endregion

        #region Update

      

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <param name="updateAction">用来改变实体值的Action</param>
        /// <returns>更新后的实体</returns>
        TEntity Update(TPrimaryKey id, Action<TEntity> updateAction);

        /// <summary>
        /// 更新一个已存在的实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        /// <param name="updateAction">用来改变实体值的Action</param>
        /// <returns>更新后的实体</returns>
        Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction);

        #endregion

        #region Delete

        

        /// <summary>
        /// 通过主键来删除实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        void Delete(TPrimaryKey id);

        /// <summary>
        /// 通过主键来删除实体
        /// </summary>
        /// <param name="id">实体的主键</param>
        Task DeleteAsync(TPrimaryKey id);

      
        #endregion

        #region 聚合操作 Aggregates
        #endregion
    }
}