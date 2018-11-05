using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepositoryManager
    {
        TRepository GetRepository<TEntity, TRepository>() where TEntity : class, IEntity where TRepository : class;
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        
        IRepositoryOfTreeEntity<TEntity> GetTreeRepository<TEntity>() where TEntity : class;
    }
}