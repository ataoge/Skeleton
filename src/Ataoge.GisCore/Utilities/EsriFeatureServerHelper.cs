using System;
using Esri = Ataoge.GisCore.FeatureServer;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Ataoge.GisCore.Utilities
{
    public static class EsriFeatureServerHelper
    {
        public static Esri.FieldInfo[] BuildFieldInfosFromType(Type type)
        {
            var fieldInfos = new List<Esri.FieldInfo>();
            foreach(var propertyInfo in type.GetProperties())
            {
                //跳过Geometry
                if (propertyInfo.Name.Equals("Geometry", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                var fieldInfo = new Esri.FieldInfo();
                fieldInfo.Name = GetJsonNameByProperty(propertyInfo);
                fieldInfo.Alias = fieldInfo.Name;
                fieldInfo.Domain = null;
                fieldInfo.Type = GetEsriFieldTypeForProperty(propertyInfo);
                switch(fieldInfo.Type)
                {
                    case EsriFieldType.esriFieldTypeOID:
                        fieldInfo.Editable = false;
                        fieldInfo.Nullable = false;
                        break;
                    case EsriFieldType.esriFieldTypeGlobalID:
                        fieldInfo.Editable = false;
                        fieldInfo.Nullable = false;
                        break;
                    case EsriFieldType.esriFieldTypeString:
                        fieldInfo.Length = 255;
                        break;
                    default:
                        break;
                }

                if (fieldInfo.Type != EsriFieldType.esriFieldTypeUnsupport)
                    fieldInfos.Add(fieldInfo);
            }

            return fieldInfos.ToArray();
        }

        private static string GetJsonNameByProperty(PropertyInfo property)
        {
            Newtonsoft.Json.JsonPropertyAttribute attr = (JsonPropertyAttribute)Attribute.GetCustomAttribute(property, typeof(JsonPropertyAttribute));
            if (attr != null)
                return attr.PropertyName;
            else    
                return property.Name.ToCamelCase();
        }

        public static Esri.BaseFieldInfo[] BuildBaseFieldInfosFromType(Type type)
        {
            var fieldInfos = new List<Esri.BaseFieldInfo>();
            foreach(var propertyInfo in type.GetProperties())
            {
                //跳过Geometry
                if (propertyInfo.Name.Equals("Geometry", StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }
                
                var fieldInfo = new Esri.BaseFieldInfo();
                fieldInfo.Name = GetJsonNameByProperty(propertyInfo);
                fieldInfo.Alias = fieldInfo.Name;
                fieldInfo.Type = GetEsriFieldTypeForProperty(propertyInfo);
                switch(fieldInfo.Type)
                {
                    case EsriFieldType.esriFieldTypeString:
                        fieldInfo.Length = 255;
                        break;
                    default:
                        break;
                }
                
                if (fieldInfo.Type != EsriFieldType.esriFieldTypeUnsupport)
                    fieldInfos.Add(fieldInfo);
            }

            return fieldInfos.ToArray();
        }

        ///<summary>
        ///
        ///</summary>
        /// <param name="type">The latitude.</param>
        /// <param name="flags">0 objectIdField, 1 globalIdField, 2 displayField, 3 typeIdField, 4 subTypeField</param>
        public static string GetFieldNameFromType(Type type, int flags)
        {
            switch (flags)
            {
                case 0:
                    var propertyInfo = type.GetProperties().FirstOrDefault(p => p.Name.Equals("Id", StringComparison.InvariantCultureIgnoreCase));
                    if (propertyInfo != null)
                    {
                        return GetJsonNameByProperty(propertyInfo);
                    }
                    return "id";
                case 1:
                    if (type.GetProperties().FirstOrDefault(p => p.Name.Equals("GlobalId", StringComparison.InvariantCultureIgnoreCase)) != null)
                        return "globalid";
                    return "";
                case 2:
                    if (type.GetProperties().FirstOrDefault(p => p.Name.Equals("Title", StringComparison.InvariantCultureIgnoreCase)) != null)
                        return "title";
                    break;
                default:
                    break;
            }
            return "";
        }

        private static EsriFieldType GetEsriFieldTypeForProperty(PropertyInfo propertyInfo)
        {
            var typeCode = Type.GetTypeCode(propertyInfo.PropertyType);
            switch(typeCode)
            {
                case TypeCode.Boolean:
                    return EsriFieldType.esriFieldTypeUnsupport;
                case TypeCode.String:
                    if (propertyInfo.Name.Equals("globalid", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return EsriFieldType.esriFieldTypeGlobalID;
                    }
                    return EsriFieldType.esriFieldTypeString;
                case TypeCode.Int32:
                    if (propertyInfo.Name.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                    {
                        return EsriFieldType.esriFieldTypeOID;
                    }
                    return EsriFieldType.esriFieldTypeInteger; 
                case TypeCode.Int16:
                    return EsriFieldType.esriFieldTypeSmallInteger;
                case TypeCode.DateTime:
                    return EsriFieldType.esriFieldTypeDate;
                case TypeCode.Single:
                    return EsriFieldType.esriFieldTypeSingle;
                case TypeCode.Double:
                    return EsriFieldType.esriFieldTypeDouble;
                
                default:
                    return EsriFieldType.esriFieldTypeUnsupport;
            }
        }

        private static string ToCamelCase(this string value)
        {
            return char.ToLower(value[0]) + value.Substring(1);
        }
    }
}