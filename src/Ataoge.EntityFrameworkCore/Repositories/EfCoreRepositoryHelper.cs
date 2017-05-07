using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using Ataoge.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Ataoge.EntityFrameworkCore.Repositories
{
    internal static class EfCoreRepositoryHelper
    {
        public static IPageResult<TEntity> GetSome<TEntity>(IQueryable<TEntity> table, IEntityType entityType, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryFunc, IPageInfo pageInfo, params string [] metaData)
            where TEntity : class, IEntity
        {
            IQueryable<TEntity> qq = table; 
            if (queryFunc != null)
                qq = queryFunc(qq);
            int count = 0;
            if (pageInfo!= null && pageInfo.Index > 0)
            {
                if (pageInfo.ReturnRecordCount) 
                {
                    pageInfo.RecordCount = qq.Count();
                    count = pageInfo.RecordCount;
                }
                qq.Skip((pageInfo.Index - 1)* pageInfo.Size).Take(pageInfo.Size);
            }
            
            foreach(var metadataPath  in metaData)
            {
                qq = qq.Include(metadataPath);
            }

            return new QueryablePageResult<TEntity>(qq, count, entityType);
        }

        public static IQueryable<TEntity> TreeQuery<TEntity, TKey>(IQueryable<TEntity> query, IEntityType entityType, Expression<Func<TEntity, bool>> startQuery, Expression<Func<TEntity, bool>> whereQuery = null, bool upSearch = false, string orderByProperty = null, int level = 0) 
            where TEntity: class, IEntity<TKey> //ITreeEntity<TKey>
            //where TKey : struct, IEquatable<TKey>
        {
           
            var tableAnnn = entityType.FindAnnotation("Relational:TableName");
            string tableName = tableAnnn?.Value.ToString();
            var anno = entityType.FindProperty(nameof(ITreeEntity<int>.Pid)).FindAnnotation("Relational:ColumnName");
            string parentFieldName = anno != null ? anno.Value.ToString() : nameof(ITreeEntity<int>.Pid);
            var idAnno = entityType.FindProperty(nameof(ITreeEntity<int>.Id)).FindAnnotation("Relational:ColumnName");
            string fieldName = idAnno != null ? idAnno.Value.ToString() : nameof(ITreeEntity<int>.Id);

            string firstQuery = query.Where(startQuery).ToSql().Replace("\r\n", " ");
            string startQueryParament = startQuery.Parameters[0].Name;
            firstQuery = firstQuery.Insert(firstQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", 0 As level");

            string secondQuery = null;
            string thirdQuery = null;
            string whereQueryParament = tableName.Substring(0,1).ToLower();
            if (whereQuery == null)
            {
                secondQuery = query.ToSql().Replace("\r\n", " ");
                thirdQuery = secondQuery;
                secondQuery = secondQuery.Insert(secondQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", mytree.level + 1");
                if (upSearch)
                {
                    secondQuery += string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, fieldName, parentFieldName);
                }
                else
                {
                    secondQuery += string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, parentFieldName, fieldName);
                }
            }
            else
            {
                secondQuery = query.Where(whereQuery).ToSql().Replace("\r\n", " ");
                thirdQuery = secondQuery;
                whereQueryParament = startQuery.Parameters[0].Name;
                if (whereQueryParament == startQueryParament)
                {
                    string forReplace = "\"" + startQueryParament + "\"";
                    whereQueryParament += startQueryParament;
                    string toReplace = "\"" + whereQueryParament + "\"";
                    secondQuery = secondQuery.Replace(forReplace, toReplace);
                }
                secondQuery = secondQuery.Insert(secondQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", mytree.level + 1");
                if (upSearch)
                {
                    secondQuery = secondQuery.Insert(secondQuery.IndexOf(" WHERE", StringComparison.CurrentCultureIgnoreCase), string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament,  fieldName, parentFieldName));
                }
                else
                {
                    secondQuery = secondQuery.Insert(secondQuery.IndexOf(" WHERE", StringComparison.CurrentCultureIgnoreCase), string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, parentFieldName, fieldName));
                }
            
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("with recursive mytree as ( ");
            sqlBuilder.Append(firstQuery);
            sqlBuilder.Append(" union all ");
            sqlBuilder.Append(secondQuery);
            sqlBuilder.Append(" order by level desc");
            if (!string.IsNullOrEmpty(orderByProperty))
            {
                var orderByAnno = entityType.FindProperty(orderByProperty).FindAnnotation("Relational:ColumnName");
                string orderByFieldName = orderByAnno!=null? orderByAnno.Value.ToString() : string.Format("\"{0}\"", orderByProperty);
                sqlBuilder.Append(string.Format(", {0}", orderByFieldName));
            }
            sqlBuilder.Append(") ");

            thirdQuery = thirdQuery.Replace(tableName, "mytree");
            sqlBuilder.Append(thirdQuery);
            if (level > 0)
            {
                if (whereQuery == null)
                    sqlBuilder.Append(" where ");
                else
                    sqlBuilder.Append(" and ");
                sqlBuilder.Append(string.Format("level = {0}", level));
            }

            return query.FromSql(sqlBuilder.ToString());
        }

        public static IQueryable<TEntity> TreeQuery<TEntity, TKey>(IRepositoryHelper repositoryHelper, IQueryable<TEntity> query, IEntityType entityType, Expression<Func<TEntity, bool>> startQuery, Expression<Func<TEntity, bool>> whereQuery = null, bool upSearch = false, string orderByProperty = null, int level = 0) 
            where TEntity: class, IEntity<TKey> //ITreeEntity<TKey>
            //where TKey : struct, IEquatable<TKey>
        {
           
            var tableAnnn = entityType.FindAnnotation("Relational:TableName");
            string tableName = tableAnnn?.Value.ToString();
            var anno = entityType.FindProperty(nameof(ITreeEntity.Pid)).FindAnnotation("Relational:ColumnName");
            string parentFieldName = anno != null ? anno.Value.ToString() : nameof(ITreeEntity<int>.Pid);
            var idAnno = entityType.FindProperty(nameof(ITreeEntity.Id)).FindAnnotation("Relational:ColumnName");
            string fieldName = idAnno != null ? idAnno.Value.ToString() : nameof(ITreeEntity<int>.Id);


            List<DbParameter> dbParameters = new List<DbParameter>();
            SqlWithParameters firstSqls = query.Where(startQuery).GetSqlTextWithParement();
            AddDbParameter(repositoryHelper, dbParameters, firstSqls);

            string firstQuery = firstSqls.Sql.Replace("\r\n", " ");
            string startQueryParament = startQuery.Parameters[0].Name;
            firstQuery = firstQuery.Insert(firstQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", 0 As level");

            string secondQuery = null;
            string thirdQuery = null;
            string whereQueryParament = tableName.Substring(0,1).ToLower();
            if (whereQuery == null)
            {
                secondQuery = query.ToSql().Replace("\r\n", " ");
                thirdQuery = secondQuery;
                secondQuery = secondQuery.Insert(secondQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", mytree.level + 1");
                if (upSearch)
                {
                    secondQuery += string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, fieldName, parentFieldName);
                }
                else
                {
                    secondQuery += string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, parentFieldName, fieldName);
                }
            }
            else
            {
                SqlWithParameters secondSqls = query.Where(whereQuery).GetSqlTextWithParement();
                AddDbParameter(repositoryHelper, dbParameters, secondSqls);

                secondQuery = secondSqls.Sql.Replace("\r\n", " ");
                thirdQuery = secondQuery;
                whereQueryParament = startQuery.Parameters[0].Name;
                if (whereQueryParament == startQueryParament)
                {
                    string forReplace = "\"" + startQueryParament + "\"";
                    whereQueryParament += startQueryParament;
                    string toReplace = "\"" + whereQueryParament + "\"";
                    secondQuery = secondQuery.Replace(forReplace, toReplace);
                }
                secondQuery = secondQuery.Insert(secondQuery.IndexOf(" FROM", StringComparison.CurrentCultureIgnoreCase), ", mytree.level + 1");
                if (upSearch)
                {
                    secondQuery = secondQuery.Insert(secondQuery.IndexOf(" WHERE", StringComparison.CurrentCultureIgnoreCase), string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament,  fieldName, parentFieldName));
                }
                else
                {
                    secondQuery = secondQuery.Insert(secondQuery.IndexOf(" WHERE", StringComparison.CurrentCultureIgnoreCase), string.Format(" join mytree on \"{0}\".\"{1}\" = \"mytree\".\"{2}\" ", whereQueryParament, parentFieldName, fieldName));
                }
            
            }

            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append("with recursive mytree as ( ");
            sqlBuilder.Append(firstQuery);
            sqlBuilder.Append(" union all ");
            sqlBuilder.Append(secondQuery);
            sqlBuilder.Append(" order by level desc");
            if (!string.IsNullOrEmpty(orderByProperty))
            {
                var orderByAnno = entityType.FindProperty(orderByProperty).FindAnnotation("Relational:ColumnName");
                string orderByFieldName = orderByAnno!=null? orderByAnno.Value.ToString() : string.Format("\"{0}\"", orderByProperty);
                sqlBuilder.Append(string.Format(", {0}", orderByFieldName));
            }
            sqlBuilder.Append(") ");

            thirdQuery = thirdQuery.Replace(tableName, "mytree");
            sqlBuilder.Append(thirdQuery);
            if (level > 0)
            {
                if (whereQuery == null)
                    sqlBuilder.Append(" where ");
                else
                    sqlBuilder.Append(" and ");
                sqlBuilder.Append(string.Format("level = {0}", repositoryHelper.CreateParameterName("level")));
                AddDbParameter(repositoryHelper, dbParameters, "level", level);
            }

            //外部where
             //   var pp = db.Database.GetDbConnection().CreateCommand().CreateParameter();
            //    pp.ParameterName ="tid";
            //    pp.Value = 1;

            return query.FromSql(sqlBuilder.ToString(), dbParameters.ToArray());
        }

        private  static void AddDbParameter(IRepositoryHelper repositoryHelper, List<DbParameter> dbParameters, SqlWithParameters sqlParameter)
        {
            
             if (sqlParameter.Parameters != null && sqlParameter.Parameters.Count() > 0)
            {
                foreach(var kvParamter in sqlParameter.Parameters)
                {
                    var dbParameter = repositoryHelper.CreateDbParmeter(kvParamter.Key, kvParamter.Value);
            
                    dbParameters.Add(dbParameter);
                }
            }
        }

        private  static void AddDbParameter(IRepositoryHelper repositoryHelper, List<DbParameter> dbParameters, string paramName, object value)
        {
            var dbParameter = repositoryHelper.CreateDbParmeter(paramName, value); //dbCommand.CreateParameter();
            //dbParameter.ParameterName = paramName;
            //dbParameter.Value = value;
            dbParameters.Add(dbParameter);
        }

        /// <summary>
        /// Builds the identifier equals predicate that will be used by any LINQ providers to check
        /// if the two aggregate roots are having the same identifier.
        /// </summary>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <typeparam name="TEntity">The type of the Entity.</typeparam>
        /// <param name="id">The identifier value to be checked with.</param>
        /// <returns>The generated Lambda expression.</returns>
        public static Expression<Func<TEntity, bool>> BuildIdEqualsPredicate<TKey, TEntity>(TKey id)
            where TEntity : IEntity<TKey>
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            return
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(Expression.Property(parameter, "Id"), Expression.Constant(id)),
                    parameter);
        }


        public static Expression<Func<TEntity, bool>> BuildEqualsPredicate<TEntity>(string propertyName, object value)
            where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity));
            return 
                Expression.Lambda<Func<TEntity, bool>>(
                    Expression.Equal(Expression.Property(parameter, propertyName), Expression.Constant(value)),
                    parameter);
        }
    }
}