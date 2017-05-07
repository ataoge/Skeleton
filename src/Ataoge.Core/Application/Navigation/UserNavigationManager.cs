using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Ataoge.Collections.Extensions;
using Ataoge.Dependency;
using Ataoge.Runtime.Session;
using Ataoge.Security;

namespace Ataoge.Application.Navigation
{
    internal class UserNavigationManager<TKey> : IUserNavigationManager<TKey>, ITransientDependency
        where TKey : IEquatable<TKey>
    {
        //public IPermissionChecker PermissionChecker { get; set; }

        public ISafSession<TKey> SafSession { get; set; }

        private readonly INavigationManager _navigationManager;
        //private readonly ILocalizationContext _localizationContext;
        //private readonly IIocResolver _iocResolver;

        public UserNavigationManager(
            INavigationManager navigationManager,
            ISafSession<TKey> SafSession)
            //ILocalizationContext localizationContext,
            //IIocResolver iocResolver)
        {
            _navigationManager = navigationManager;
            //_localizationContext = localizationContext;
            //_iocResolver = iocResolver;
            //PermissionChecker = NullPermissionChecker.Instance;
            SafSession = this.SafSession; //NullAbpSession.Instance;
        }

        public async Task<UserMenu> GetMenuAsync(string menuName, UserIdentifier<TKey> user)
        {
            var menuDefinition = _navigationManager.Menus.GetOrDefault(menuName);
            if (menuDefinition == null)
            {
                throw new SafException("There is no menu with given name: " + menuName);
            }

            var userMenu = new UserMenu(menuDefinition);//, _localizationContext);
            await FillUserMenuItems(user, menuDefinition.Items, userMenu.Items);
            return userMenu;
        }

        public async Task<IReadOnlyList<UserMenu>> GetMenusAsync(UserIdentifier<TKey> user)
        {
            var userMenus = new List<UserMenu>();

            foreach (var menu in _navigationManager.Menus.Values)
            {
                userMenus.Add(await GetMenuAsync(menu.Name, user));
            }

            return userMenus;
        }

        private async Task<int> FillUserMenuItems(UserIdentifier<TKey> user, IList<MenuItemDefinition> menuItemDefinitions, IList<UserMenuItem> userMenuItems)
        {
            //TODO: Can be optimized by re-using FeatureDependencyContext.

            var addedMenuItemCount = 0;

            //using (var featureDependencyContext = _iocResolver.ResolveAsDisposable<FeatureDependencyContext>())
            {
                //featureDependencyContext.Object.TenantId = user == null ? null : user.TenantId;

                foreach (var menuItemDefinition in menuItemDefinitions)
                {
                    if (menuItemDefinition.RequiresAuthentication && user == null)
                    {
                        continue;
                    }

                    if (!string.IsNullOrEmpty(menuItemDefinition.RequiredPermissionName) && (user == null))// || !(await PermissionChecker.IsGrantedAsync(user, menuItemDefinition.RequiredPermissionName))))
                    {
                        continue;
                    }

                    //if (menuItemDefinition.FeatureDependency != null &&
                    //    (AbpSession.MultiTenancySide == MultiTenancySides.Tenant || (user != null && user.TenantId != null)) &&
                    //    !(await menuItemDefinition.FeatureDependency.IsSatisfiedAsync(featureDependencyContext.Object)))
                    //{
                    //    continue;
                    //}

                    var userMenuItem = new UserMenuItem(menuItemDefinition); //, _localizationContext);
                    if (menuItemDefinition.IsLeaf || (await FillUserMenuItems(user, menuItemDefinition.Items, userMenuItem.Items)) > 0)
                    {
                        userMenuItems.Add(userMenuItem);
                        ++addedMenuItemCount;
                    }
                }
            }

            return addedMenuItemCount;
        }
    }
}