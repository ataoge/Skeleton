using System.Collections.Generic;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Providers
{
    /// <summary>
	/// Responsible for getting a collection of <see cref="IEntityFrameworkModelBuilder" />.
	/// </summary>
	public interface IEntityTypeConfigurationProvider
    {
		/// <summary>
		/// Returns a collection of <see cref="IEntityFrameworkModelBuilder" />.
		/// </summary>
		/// <returns>A collection of <see cref="IEntityFrameworkModelBuilder" />.</returns>
		IEnumerable<IEntityFrameworkModelBuilder>  GetModelBuilder(); 
    }
}