using System.Collections.Generic;
using Ataoge.Collections.Extensions;

namespace Ataoge.Application.Navigation
{
    public static class UserMenuItemExtensions
    {
        public static IEnumerable<UserMenuItem> DescendantsOrSelf(this UserMenuItem userMenuItem)
        {
            return GetDescendants(userMenuItem, true);
        }

        public static IEnumerable<UserMenuItem> Descendants(this UserMenuItem userMenuItem)
        {
            return GetDescendants(userMenuItem, false);
        }

        internal static IEnumerable<UserMenuItem> GetDescendants(this UserMenuItem userMenuItem, bool includeSelf = false)
        {
            if (includeSelf)
            {
                yield return userMenuItem;
            }

            if (!userMenuItem.Items.IsNullOrEmpty())
            {
                foreach(var child in userMenuItem.Items)
                {
                    yield return child;

                    foreach(var dd in Descendants(child))
                    {
                        yield return dd;
                    }
                }
            }

        } 
    }
} 