using System;
using System.Linq;
using System.Reflection;

namespace Ataoge.Reflection
{
    internal static class ReflectionHelper
    {
        public static TAttribute GetSingleAttributeOrDefault<TAttribute>(this MemberInfo memberInfo, TAttribute defaultValue = default(TAttribute))
            where TAttribute : Attribute
        {
            //Get attribute on the member
            if (memberInfo.IsDefined(typeof(TAttribute), true))
            {
                return memberInfo.GetCustomAttributes(typeof(TAttribute), true).Cast<TAttribute>().First();
            }

            return defaultValue;
        }
    }
}