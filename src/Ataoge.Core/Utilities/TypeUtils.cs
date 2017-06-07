using System;
using System.Linq;
using System.Reflection;

namespace Ataoge.Utilities
{
    public static class TypeUtils
    {
        public static Type FindGenericTypeArguments(Type srcType, Type definitionType, int index = 0)
        {
            TypeInfo typeInfo = srcType.GetTypeInfo();
            var genericInterface = typeInfo.GetInterfaces().FirstOrDefault(t => t.GetTypeInfo().IsGenericType && t.GetTypeInfo().GetGenericTypeDefinition() == definitionType);
            return genericInterface?.GenericTypeArguments[index];
        }
    }
}