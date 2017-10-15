using Ataoge.Data;

namespace Ataoge.Repositories
{
    public interface IRepositoryManager
    {
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        
        IRepositoryOfTreeEntity<TEntity> GetTreeRepository<TEntity>() where TEntity : class;
    }
}