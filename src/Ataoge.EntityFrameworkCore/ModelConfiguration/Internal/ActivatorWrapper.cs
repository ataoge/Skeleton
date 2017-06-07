using System;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Internal
{
    /// <summary>
	/// Wrapper class around <see cref="Activator" />.
	/// </summary>
	public class ActivatorWrapper : IActivator
	{
		/// <summary>
		/// Creates an instance of the specified type using that type's default constructor.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>A reference to the newly created object.</returns>
		public object CreateInstance( Type type )
		{
			Check.NotNull( type, nameof( type ) );


			return Activator.CreateInstance( type );
		}
	}
}