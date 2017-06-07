using Microsoft.EntityFrameworkCore;

namespace Ataoge.EntityFrameworkCore
{
    /// <summary>
    /// Formalization of entity framework DbContext model creation <see cref="DbContext.OnModelCreating"/>
    /// </summary>
    /// <remarks>
    /// The reason for this explicit interface of the <see cref="DbContext.OnModelCreating"/> is to make compositions of several model builders into one
    /// </remarks>
    public interface IEntityFrameworkModelBuilder
    {
        void BuildModel(ModelBuilder modelBuilder, AtaogeDbContext dbContext); //, ModelManager modelManager);
    }
}