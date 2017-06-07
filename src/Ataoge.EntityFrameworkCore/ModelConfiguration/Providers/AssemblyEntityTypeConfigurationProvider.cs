using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ataoge.EntityFrameworkCore.ModelConfiguration.Internal;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Providers
{
    /// <summary>
	/// Returns a collection of <see cref="IEntityTypeConfiguration" /> from the a collection of assemblies.
	/// </summary>
	public class AssemblyEntityTypeConfigurationProvider
		: IEntityTypeConfigurationProvider
	{
		/// <summary>
		/// Instaniates and instance of <see cref="AssemblyEntityTypeConfigurationProvider" />.
		/// </summary>
		/// <param name="assemblies">The collection of assemblies to search.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies" /> is null.</exception>
		public AssemblyEntityTypeConfigurationProvider( IEnumerable<Assembly> assemblies )
			: this( assemblies, new ActivatorWrapper() )
		{ }

		/// <summary>
		/// Instaniates and instance of <see cref="AssemblyEntityTypeConfigurationProvider" />.
		/// </summary>
		/// <param name="assemblies">The collection of assemblies to search.</param>
		/// <param name="activator">The activator responsible for instantiating objects.</param>
		/// <exception cref="ArgumentNullException">Thrown when <paramref name="assemblies" /> or <paramref name="activator" /> is null.</exception>
		public AssemblyEntityTypeConfigurationProvider( IEnumerable<Assembly> assemblies, IActivator activator )
		{
			Check.NotNull( assemblies, nameof( assemblies ) );

			Check.NotNull( activator, nameof( activator ) );

			this.Assemblies = assemblies;
			this.Activator = activator;
		}

		/// <summary>
		/// Gets the collection of assmeblies to search.
		/// </summary>
		public IEnumerable<Assembly> Assemblies { get; }

		/// <summary>
		/// Gets the activator responsible for creating objects.
		/// </summary>
		public IActivator Activator { get; }

        public IEnumerable<IEntityFrameworkModelBuilder>  GetModelBuilder()
		{
			return this.Assemblies.SelectMany( x => x.DefinedTypes )
				.Select( x => x.AsType() )
				.Where( x => typeof( IEntityFrameworkModelBuilder ).GetTypeInfo().IsAssignableFrom( x )  && !x.GetTypeInfo().IsAbstract)
				.Select( x => ( IEntityFrameworkModelBuilder )this.Activator.CreateInstance( x ) );
		} 
    }
}