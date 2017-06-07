using System.Reflection;
using Ataoge;
using Ataoge.EntityFrameworkCore.ModelConfiguration.Infrastructure;
using Ataoge.EntityFrameworkCore.ModelConfiguration.Providers;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
	/// Adds methods to <see cref="DbContextOptionsBuilder" /> to add support for <see cref="IEntityTypeConfiguration" />.
	/// </summary>
	public static class DbContextOptionsBuilderExtensions
	{
        /// <summary>
		/// Adds <see cref="IEntityTypeConfiguration" /> from all of the <paramref name="assemblies" /> specified.
		/// </summary>
		/// <typeparam name="TContext">The type of <see cref="DbContext" />.</typeparam>
		/// <param name="builder">The builder.</param>
		/// <param name="assemblies">The assemblies to search for <see cref="IEntityTypeConfiguration" />.</param>
		/// <returns>The builder.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies" /> is null.</exception>
		public static DbContextOptionsBuilder<TContext> AddEntityTypeConfigurations<TContext>( this DbContextOptionsBuilder<TContext> builder, params Assembly[] assemblies )
			where TContext : DbContext
		{
			Check.NotNull( builder, nameof( builder ) );

			Check.NotNull( assemblies, nameof( assemblies ) );

			( ( DbContextOptionsBuilder )builder ).AddEntityTypeConfigurations( assemblies );

			return builder;
		}

        /// <summary>
		/// Adds <see cref="IEntityTypeConfiguration" /> base on the <paramref name="provider" />.
		/// </summary>
		/// <typeparam name="TContext">The type of <see cref="DbContext" />.</typeparam>
		/// <param name="builder">The builder.</param>
		/// <param name="provider">The <see cref="IEntityTypeConfigurationProvider" />.</param>
		/// <returns>The builder.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="provider" /> is null.</exception>
		public static DbContextOptionsBuilder<TContext> AddEntityTypeConfigurations<TContext>( this DbContextOptionsBuilder<TContext> builder, IEntityTypeConfigurationProvider provider )
			where TContext : DbContext
		{
			Check.NotNull( builder, nameof( builder ) );

			Check.NotNull( provider, nameof( provider ) );

			( ( DbContextOptionsBuilder )builder ).AddEntityTypeConfigurations( provider );

			return builder;
		}

        /// <summary>
		/// Adds <see cref="IEntityTypeConfiguration" /> from all of the <paramref name="assemblies" /> specified.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="assemblies">The assemblies to search for <see cref="IEntityTypeConfiguration" />.</param>
		/// <returns>The builder.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies" /> is null.</exception>
		public static DbContextOptionsBuilder AddEntityTypeConfigurations( this DbContextOptionsBuilder builder, params Assembly[] assemblies )
		{
			Check.NotNull( builder, nameof( builder ) );

			Check.NotNull( assemblies, nameof( assemblies ) );

			AssemblyEntityTypeConfigurationProvider provider = new AssemblyEntityTypeConfigurationProvider( assemblies );

			return builder.AddEntityTypeConfigurations( provider );
		}

		/// <summary>
		/// Adds <see cref="IEntityTypeConfiguration" /> base on the <paramref name="provider" />.
		/// </summary>
		/// <param name="builder">The builder.</param>
		/// <param name="provider">The <see cref="IEntityTypeConfigurationProvider" />.</param>
		/// <returns>The builder.</returns>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="provider" /> is null.</exception>
		public static DbContextOptionsBuilder AddEntityTypeConfigurations( this DbContextOptionsBuilder builder, IEntityTypeConfigurationProvider provider )
		{
			Check.NotNull( builder, nameof( builder ) );

			Check.NotNull( provider, nameof( provider ) );


			EntityTypeConfigurationOptionsExtension optionsExtension = new EntityTypeConfigurationOptionsExtension( provider );

			( ( IDbContextOptionsBuilderInfrastructure )builder ).AddOrUpdateExtension( optionsExtension );

			return builder;
		}

			
    }
}