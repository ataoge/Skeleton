using System;
using System.Reflection;


namespace Ataoge.Data
{
    public static class SafDbTypeHelper
    {
        /// <summary>
        /// 根据对象获取类型
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>DbType <see cref="DbType"/></returns>
        public static SafDbType GetSafDbTypeForObject(object value)
        {
            if (value != null)
            {
                Type dataType;
                if (value is Type)
                    dataType = (Type)value;
                else
                    dataType = value.GetType();

                //处理Char和Char数组类型
                if (typeof(char) == dataType)
                {
                    //this._value = value.ToString();
                    dataType = typeof(string); 
                }
                else if (typeof(char[]) == dataType)
                {
                    //this._value = new string((char[])value);
                    dataType = typeof(string);
                }

                return GetSafDbTypeForType(dataType);
            }
            throw new ArgumentException("Null Value");
        }

        /// <summary>
        /// 根据对象获取类型
        /// </summary>
        /// <param name="value">对象</param>
        /// <returns>DbType <see cref="DbType"/></returns>
        public static SafDbType GetSafDbTypeForType(Type type)
        {
                switch (GetTypeCode(type))
                {
                    case TypeCode.Boolean:
                        return SafDbType.Boolean;
                    case TypeCode.Byte:
                        return SafDbType.Byte;
                    case TypeCode.Char:
                        throw new ArgumentException("Char");
                    case TypeCode.DateTime:
                        return SafDbType.DateTime;
                    //case TypeCode.DBNull:
                        //throw new ArgumentException("DBNull");
                    case TypeCode.Decimal:
                        return SafDbType.Decimal;
                    case TypeCode.Double:
                        return SafDbType.Double;
                    case TypeCode.Empty:
                        throw new ArgumentException("Empty");
                    case TypeCode.Int16:
                        return SafDbType.Int16;
                    case TypeCode.Int32:
                        return SafDbType.Int32;
                    case TypeCode.Int64:
                        return SafDbType.Int64;
                    case TypeCode.SByte:
                        return SafDbType.SByte;
                    case TypeCode.Single:
                        return SafDbType.Single;
                    case TypeCode.UInt16:
                        return SafDbType.UInt16;
                    case TypeCode.UInt32:
                        return SafDbType.UInt32;
                    case TypeCode.UInt64:
                        return SafDbType.UInt64;
                    case TypeCode.String:
                        return SafDbType.String;
                    case TypeCode.Object:
                        if (type != typeof(byte[]))
                        {

                            if (type == typeof(Guid))
                            {
                                return SafDbType.Guid;
                            }
                            if (type == typeof(object))
                            {
                                return SafDbType.Object;
                            }
                        }
                        return SafDbType.Binary;
                    default:
                        return SafDbType.Object;
                }
                throw new ArgumentException("Unknow Type");          
        }

        private static TypeCode GetTypeCode(Type type)
        {
            if (type.GetTypeInfo().IsGenericType)
            {
                var genericTypes =   type.GetTypeInfo().GenericTypeArguments;
                if(genericTypes!=null&&genericTypes.Length>0)
                {
                    return  Type.GetTypeCode(genericTypes[0]);
                }
            }
            return Type.GetTypeCode(type);
  
        }
    }
}