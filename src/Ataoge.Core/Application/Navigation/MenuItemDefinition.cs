using System.Collections.Generic;
using Ataoge.Collections.Extensions;

namespace Ataoge.Application.Navigation
{
    /// <summary>
    /// Represents an item in a <see cref="MenuDefinition"/>.
    /// </summary>
    public class MenuItemDefinition : IHasMenuItemDefinitions
    {
        /// <summary>
        /// Unique name of the menu item in the application. 
        /// Can be used to find this menu item later.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Display name of the menu item. Required.
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// The Display order of the menu. Optional.
        /// </summary>
        public int Order { get; set; }
        
        /// <summary>
        /// Icon of the menu item if exists. Optional.
        /// </summary>
        public string Icon { get; set; }
        
        /// <summary>
        /// The URL to navigate when this menu item is selected. Optional.
        /// </summary>
        public string Url { get; set; }

         /// <summary>
        /// A permission name. Only users that has this permission can see this menu item.
        /// Optional.
        /// </summary>
        public string RequiredPermissionName { get; set; }

        /// <summary>
        /// This can be set to true if only authenticated users should see this menu item.
        /// </summary>
        public bool RequiresAuthentication { get; set; }

        /// <summary>
        /// Returns true if this menu item has no child <see cref="Items"/>.
        /// </summary>
        public bool IsLeaf => Items.IsNullOrEmpty();
        
        /// <summary>
        /// Target of the menu item. Can be "_blank", "_self", "_parent", "_top" or a frame name.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// Can be used to store a custom object related to this menu item. Optional.
        /// </summary>
        public object CustomData { get; set; }

        /// <summary>
        /// Can be used to enable/disable a menu item.
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// Can be used to show/hide a menu item.
        /// </summary>
        public bool IsVisible { get; set; }

        /// <summary>
        /// Sub items of this menu item. Optional.
        /// </summary>
        public virtual IList<MenuItemDefinition> Items { get; private set; }

        /// <summary>
        /// Creates a new <see cref="MenuItemDefinition"/> object.
        /// </summary>
        public MenuItemDefinition(
            string name, 
            string displayName, 
            string icon = null, 
            string url = null, 
            bool requiresAuthentication = false, 
            string requiredPermissionName = null, 
            int order = 0, 
            object customData = null,
            string target = null,
            bool isEnabled = true,
            bool isVisible = true)
        {
            Check.NotNull(name, nameof(name));
            Check.NotNull(displayName, nameof(displayName));

            Name = name;
            DisplayName = displayName;
            Icon = icon;
            Url = url;
            RequiresAuthentication = requiresAuthentication;
            RequiredPermissionName = requiredPermissionName;
            Order = order;
            CustomData = customData;
            //FeatureDependency = featureDependency;
            Target = target;
            IsEnabled = isEnabled;
            IsVisible = isVisible;

            Items = new List<MenuItemDefinition>();
        }

         /// <summary>
        /// Adds a <see cref="MenuItemDefinition"/> to <see cref="Items"/>.
        /// </summary>
        /// <param name="menuItem"><see cref="MenuItemDefinition"/> to be added</param>
        /// <returns>This <see cref="MenuItemDefinition"/> object</returns>
        public MenuItemDefinition AddItem(MenuItemDefinition menuItem)
        {
            Items.Add(menuItem);
            return this;
        }
    }
}