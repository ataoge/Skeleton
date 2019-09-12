using System;
using System.Data;
using System.Reflection;

namespace Ataoge.Data.Mapper
{
    /// <summary>
        /// Implement this interface to change default mapping of reader columns to type members
        /// </summary>
        public interface ITypeMap
        {
            /// <summary>
            /// Finds best constructor
            /// </summary>
            /// <param name="names">DataReader column names</param>
            /// <param name="types">DataReader column types</param>
            /// <returns>Matching constructor or default one</returns>
            ConstructorInfo FindConstructor(string[] names, Type[] types);

            /// <summary>
            /// Returns a constructor which should *always* be used.
            /// 
            /// Parameters will be default values, nulls for reference types and zero'd for value types.
            /// 
            /// Use this class to force object creation away from parameterless constructors you don't control.
            /// </summary>
            ConstructorInfo FindExplicitConstructor();

            /// <summary>
            /// Gets mapping for constructor parameter
            /// </summary>
            /// <param name="constructor">Constructor to resolve</param>
            /// <param name="columnName">DataReader column name</param>
            /// <returns>Mapping implementation</returns>
            IMemberMap GetConstructorParameter(ConstructorInfo constructor, string columnName);

            /// <summary>
            /// Gets member mapping for column
            /// </summary>
            /// <param name="columnName">DataReader column name</param>
            /// <returns>Mapping implementation</returns>
            IMemberMap GetMember(string columnName);
        }

        /// <summary>
        /// Implements this interface to provide custom member mapping
        /// </summary>
        public interface IMemberMap
        {
            /// <summary>
            /// Source DataReader column name
            /// </summary>
            string ColumnName { get; }

            /// <summary>
            ///  Target member type
            /// </summary>
            Type MemberType { get; }

            /// <summary>
            /// Target property
            /// </summary>
            PropertyInfo Property { get; }

            /// <summary>
            /// Target field
            /// </summary>
            FieldInfo Field { get; }

            /// <summary>
            /// Target constructor parameter
            /// </summary>
            ParameterInfo Parameter { get; }
        }

        /// <summary>
        /// Implement this interface to perform custom type-based parameter handling and value parsing
        /// </summary>
        public interface ITypeHandler
        {
            /// <summary>
            /// Assign the value of a parameter before a command executes
            /// </summary>
            /// <param name="parameter">The parameter to configure</param>
            /// <param name="value">Parameter value</param>
            void SetValue(IDbDataParameter parameter, object value);

            /// <summary>
            /// Parse a database value back to a typed value
            /// </summary>
            /// <param name="value">The value from the database</param>
            /// <param name="destinationType">The type to parse to</param>
            /// <returns>The typed value</returns>
            object Parse(Type destinationType, object value);
        }
}