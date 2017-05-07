using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ataoge.Security;

namespace Ataoge.Application.Navigation
{
    /// <summary>
    /// Used to manage navigation for users.
    /// </summary>
    public interface IUserNavigationManager<TKey>
        where TKey: IEquatable<TKey>
    {
        /// <summary>
        /// Gets a menu specialized for given user.
        /// </summary>
        /// <param name="menuName">Unique name of the menu</param>
        /// <param name="user">The user, or null for anonymous users</param>
        Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier<TKey> user);

        /// <summary>
        /// Gets all menus specialized for given user.
        /// </summary>
        /// <param name="user">User id or null for anonymous users</param>
        Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier<TKey> user);
    }
}