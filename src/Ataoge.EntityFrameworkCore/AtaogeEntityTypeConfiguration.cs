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
            if (string.IsNullOrEmpty(name))
                throw new System.ArgumentNullException(nameof(name));

            string providerName = _ataogeDbContext?.ProviderName;
            switch(name)
            {
                case "Npgsql.EntityFrameworkCore.PostgreSQL":
                    return name.ToLower();
                default:
                    return name;
            }
        }

        public abstract void Configure(EntityTypeBuilder<TEntity> builder);
        
    }
}