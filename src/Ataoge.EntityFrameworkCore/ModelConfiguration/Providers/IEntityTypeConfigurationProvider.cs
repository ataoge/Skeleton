using System.Collections.Generic;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Providers
{
    /// <summary>
	/// Responsible for getting a collection of <see cref="IEntityTypeConfiguration" />.
	/// </summary>
	public interface IEntityTypeConfigurationProvider
    {
		/// <summary>
		/// Returns a collection of <see cref="IEntityTypeConfiguration" />.
		/// </summary>
		/// <returns>A collection of <see cref="IEntityTypeConfiguration" />.</returns>
		//IEnumerable<IEntityTypeConfiguration> GetConfigurations();

		//IEnumerable<IConfigurationFactory> GetFactories();

		IEnumerable<IEntityFrameworkModelBuilder>  GetModelBuilder(); 
    }
}