using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace Microsoft.EntityFrameworkCore
{
    public static class RDFacadeExtensions
    {
        public static RelationalDataReader ExecuteSqlQuery(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
        {
            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return rawSqlCommand
                    .RelationalCommand
                    .ExecuteReader(
                        databaseFacade.GetService<IRelationalConnection>(),
                        parameterValues: rawSqlCommand.ParameterValues);
            }
        }

        public static async Task<RelationalDataReader> ExecuteSqlCommandAsync(this DatabaseFacade databaseFacade,
                                                             string sql,
                                                             CancellationToken cancellationToken = default(CancellationToken),
                                                             params object[] parameters)
        {

            var concurrencyDetector = databaseFacade.GetService<IConcurrencyDetector>();

            using (concurrencyDetector.EnterCriticalSection())
            {
                var rawSqlCommand = databaseFacade
                    .GetService<IRawSqlCommandBuilder>()
                    .Build(sql, parameters);

                return await rawSqlCommand
                    .RelationalCommand
                    .ExecuteReaderAsync(
                        databaseFacade.GetService<IRelationalConnection>(),
                        parameterValues: rawSqlCommand.ParameterValues,
                        cancellationToken: cancellationToken);
            }
        }
   
        public static IEnumerable<T> GetModelFromQuery<T>(this DatabaseFacade databaseFacade, Func<T> newFunc,  string sql, params object[] parameters)
        {
            using (DbDataReader dr = databaseFacade.ExecuteSqlQuery(sql, parameters).DbDataReader)
            {
                List<T> lst = new List<T>();
                PropertyInfo[] props = typeof(T).GetTypeInfo().GetProperties();
                while (dr.Read())
                {
                    T t = newFunc();
                    IEnumerable<string> actualNames = dr.GetColumnSchema().Select(o => o.ColumnName);
                    for (int i = 0; i < props.Length; ++i)
                    {
                        PropertyInfo pi = props[i];

                        if (!pi.CanWrite) continue;

                        System.ComponentModel.DataAnnotations.Schema.ColumnAttribute ca = pi.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)) as System.ComponentModel.DataAnnotations.Schema.ColumnAttribute;
                        string name = ca?.Name ?? pi.Name;

                        if (pi == null) continue;

                        if (!actualNames.Contains(name))
                        {
                            continue;
                        }
                        object value = dr[name];
                        Type pt = pi.DeclaringType;
                        bool nullable = pt.GetTypeInfo().IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>);
                        if (value == DBNull.Value)
                        {
                            value = null;
                        }
                        if (value == null && pt.GetTypeInfo().IsValueType && !nullable)
                        {
                            value = Activator.CreateInstance(pt);
                        }
                        pi.SetValue(t, value);
                    }//for i
                    lst.Add(t);
                }//while
                return lst;
            }//using dr
        }

        public static IEnumerable<T> GetModelFromQuery<T>(this DatabaseFacade databaseFacade, string sql, params object[] parameters)
                where T : new()
        {
            return GetModelFromQuery(databaseFacade, ()=> new T(), sql, parameters);
            /*using (DbDataReader dr = databaseFacade.ExecuteSqlQuery(sql, parameters).DbDataReader)
            {
                List<T> lst = new List<T>();
                PropertyInfo[] props = typeof(T).GetTypeInfo().GetProperties();
                while (dr.Read())
                {
                    T t = new T();
                    IEnumerable<string> actualNames = dr.GetColumnSchema().Select(o => o.ColumnName);
                    for (int i = 0; i < props.Length; ++i)
                    {
                        PropertyInfo pi = props[i];

                        if (!pi.CanWrite) continue;

                        System.ComponentModel.DataAnnotations.Schema.ColumnAttribute ca = pi.GetCustomAttribute(typeof(System.ComponentModel.DataAnnotations.Schema.ColumnAttribute)) as System.ComponentModel.DataAnnotations.Schema.ColumnAttribute;
                        string name = ca?.Name ?? pi.Name;

                        if (pi == null) continue;

                        if (!actualNames.Contains(name))
                        {
                            continue;
                        }
                        object value = dr[name];
                        Type pt = pi.DeclaringType;
                        bool nullable = pt.GetTypeInfo().IsGenericType && pt.GetGenericTypeDefinition() == typeof(Nullable<>);
                        if (value == DBNull.Value)
                        {
                            value = null;
                        }
                        if (value == null && pt.GetTypeInfo().IsValueType && !nullable)
                        {
                            value = Activator.CreateInstance(pt);
                        }
                        pi.SetValue(t, value);
                    }//for i
                    lst.Add(t);
                }//while
                return lst;
            }//using dr */
        }
       
     }
}


//Usage:

// Execute a query.
//var dr= await db.Database.ExecuteSqlQueryAsync("SELECT \"ID\", \"Credits\", \"LoginDate\", (select count(DISTINCT \"MapID\") from \"SampleBase\") as \"MapCount\" " +
//                                                       "FROM \"SamplePlayer\" " +
//                                                       "WHERE " +
//                                                          "\"Name\" IN ('Electro', 'Nitro')");

// Output rows.
//while (dr.Read())
//  Console.Write("{0}\t{1}\t{2}\t{3} \n", dr[0], dr[1], dr[2], dr[3]);

// Don't forget to dispose the DataReader! 
//dr.Dispose();