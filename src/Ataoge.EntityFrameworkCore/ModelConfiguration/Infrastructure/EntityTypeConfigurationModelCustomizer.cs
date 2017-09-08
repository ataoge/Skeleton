using Ataoge.EntityFrameworkCore.ModelConfiguration.Providers;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Infrastructure
{
    /// <summary>
	/// Configures the entities by first getting the collection of <see cref="IEntityTypeConfiguration" /> from the <see cref="Provider" />.
	/// Then proceeds with the default implementation of <see cref="ModelCustomizer" />.
	/// </summary>
	public class EntityTypeConfigurationModelCustomizer
		: RelationalModelCustomizer
    {
		/// <summary>
		/// Instantiates an instance of <see cref="EntityTypeConfigurationModelCustomizer" />.
		/// </summary>
		/// <param name="provider">The provider used to get the collection of <see cref="IEntityTypeConfiguration" />.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="provider" /> is null.</exception>
		public EntityTypeConfigurationModelCustomizer(IEntityTypeConfigurationProvider provider, [NotNull] ModelCustomizerDependencies dependencies)
            : base(dependencies)
		{
			Check.NotNull( provider, nameof( provider ) );
			
			this.Provider = provider;
		}

        /// <summary>
		/// Returns the <see cref="IEntityTypeConfigurationProvider" />.
		/// </summary>
		public IEntityTypeConfigurationProvider Provider { get; }

        /// <summary>
		/// Configures the entities by first getting the collection of <see cref="IEntityTypeConfiguration" /> from the <see cref="Provider" />.
		/// Then proceeds with the default implementation of <see cref="ModelCustomizer" />.
		/// </summary>
		/// <param name="modelBuilder">The builder being used to construct the model.</param>
		/// <param name="dbContext">The context instance that the model is being created for.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="modelBuilder" /> or <paramref name="dbContext" /> is null.</exception>
		public override void Customize(ModelBuilder modelBuilder, DbContext dbContext )
		{
			Check.NotNull( modelBuilder, nameof( modelBuilder ) );

			foreach (var efModelBuilder in this.Provider.GetModelBuilder())
			{
				efModelBuilder.BuildModel(modelBuilder, dbContext as AtaogeDbContext);
			}

            base.Customize( modelBuilder, dbContext );
		}
	}
}