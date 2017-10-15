using System;
using Ataoge.Data;

namespace Ataoge.Repositories.Internal
{
    public class RepositoryManager : IRepositoryManager
    {
        public RepositoryManager(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        protected IServiceProvider ServiceProvider {get;}

        private readonly Type DbTableAttributeType = typeof(DbTableAttribute);

        public IRepositoryOfTreeEntity<TEntity> GetTreeRepository<TEntity>() where TEntity : class
        {
            Type type = typeof(TEntity);
            DbTableAttribute attr = (DbTableAttribute)Attribute.GetCustomAttribute(type, DbTableAttributeType);
            if (attr == null || attr.RepostitoryInterface == null)
                return null;
            return this.ServiceProvider.GetService(attr.RepostitoryInterface)  as IRepositoryOfTreeEntity<TEntity>;
        }

        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity
        {
            Type type = typeof(TEntity);
            DbTableAttribute attr = (DbTableAttribute)Attribute.GetCustomAttribute(type, DbTableAttributeType);
            if (attr == null || attr.RepostitoryInterface == null)
                return null;
            return this.ServiceProvider.GetService(attr.RepostitoryInterface)  as IRepository<TEntity>;
        }
    }
}