using Ataoge.Data;
using Ataoge.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microsoft.EntityFrameworkCore
{
    public abstract class AtaogeEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        protected AtaogeEntityTypeConfiguration(IAtaogeDbContext dbContext)
        {
            _ataogeDbContext = dbContext;
        }

        private readonly IAtaogeDbContext _ataogeDbContext;

        protected string  ConvertName(string name)
        {
            string providerName = _ataogeDbContext?.ProviderName;
            return RDFacadeExtensions.ConvertName(providerName, name);
        }

        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
        
    }
}