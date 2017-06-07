using Ataoge.EntityFrameworkCore.ModelConfiguration.Providers;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Infrastructure
{
    /// <summary>
	/// The <see cref="IDbContextOptionsExtension" /> which configures the services neccessary to support configuring entities with <see cref="IEntityTypeConfiguration" />.
	/// </summary>
	public class EntityTypeConfigurationOptionsExtension
		: IDbContextOptionsExtension
	{
        /// <summary>
		/// Instantiates an instance of <see cref="EntityTypeConfigurationOptionsExtension" />.
		/// </summary>
		/// <param name="provider">The <see cref="IEntityTypeConfigurationProvider" />.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="provider" /> is null.</exception>
		public EntityTypeConfigurationOptionsExtension( IEntityTypeConfigurationProvider provider )
		{
			Check.NotNull( provider, nameof( provider ) );

			this.Provider = provider;
		}

		/// <summary>
		/// Gets the <see cref="IEntityTypeConfigurationProvider" />.
		/// </summary>
		public IEntityTypeConfigurationProvider Provider { get; }

        /// <summary>
		/// Replaces the <see cref="IModelCustomizer" /> implementation with <see cref="EntityTypeConfigurationModelCustomizer" />
		/// and adds the <see cref="IEntityTypeConfigurationProvider" /> to the <paramref name="services" />.
		/// </summary>
		/// <param name="services">The collection to add services to.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="services" /> is null.</exception>
		public void ApplyServices( IServiceCollection services )
		{
			Check.NotNull( services, nameof( services ) );

            ServiceDescriptor modelCustomizerServiceDescriptor = ServiceDescriptor.Singleton<IModelCustomizer, EntityTypeConfigurationModelCustomizer>();
			services.Replace( modelCustomizerServiceDescriptor );
			
			services.AddSingleton<IEntityTypeConfigurationProvider>( this.Provider );
		}
	}
}