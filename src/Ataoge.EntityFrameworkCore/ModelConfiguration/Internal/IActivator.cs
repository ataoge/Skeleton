using System;

namespace Ataoge.EntityFrameworkCore.ModelConfiguration.Internal
{
    /// <summary>
	/// Interface to abstract <see cref="Activator.CreateInstance(Type)" />.
	/// </summary>
    public interface IActivator
    {
		/// <summary>
		/// Creates an instance of the specified type using that type's default constructor.
		/// </summary>
		/// <param name="type">The type of object to create.</param>
		/// <returns>A reference to the newly created object.</returns>
		object CreateInstance( Type type );
    }
}