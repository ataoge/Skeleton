using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Remotion.Linq.Parsing.Structure;

namespace System.Linq
{
    public static class AtaogeQueryableExtensions
    {
        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");
 

        private static readonly MethodInfo ExtractParametersMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "ExtractParameters");
        private static readonly FieldInfo QueryContextFactoryField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_queryContextFactory");


        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

        public static string ToSql<TEntity>(this IQueryable<TEntity> query, bool useParameters = false) where TEntity : class
        {
            var selectExpression = GetSelect(query, useParameters);
            return selectExpression?.ToString();
            //return sql;
        }

        public static SelectExpression GetSelect<TEntity>(this IQueryable<TEntity> query, bool useParameters = false) where TEntity : class
        {
            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
            {
                throw new ArgumentException("Invalid query");
            }

            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);

            Expression expression = query.Expression;
            if (useParameters)  //采用参数模式替换
            {
                var queryContextFactory=(IQueryContextFactory)QueryContextFactoryField.GetValue(queryCompiler);
                var queryContext = queryContextFactory.Create();
                expression = (Expression) ExtractParametersMethod.Invoke(queryCompiler, new object[] {query.Expression, queryContext});
            }

            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            
            var queryModel = parser.GetParsedQuery(expression);//query.Expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

            return modelVisitor.Queries.First();
        }

        internal static SqlWithParameters GetSqlTextWithParement<TEntity>(this IQueryable<TEntity> query) where TEntity : class
        {
            if (!(query is EntityQueryable<TEntity>) && !(query is InternalDbSet<TEntity>))
            {
                throw new ArgumentException("Invalid query");
            }

            var queryCompiler = (IQueryCompiler)QueryCompilerField.GetValue(query.Provider);

            var queryContextFactory=(IQueryContextFactory)QueryContextFactoryField.GetValue(queryCompiler);
            var queryContext = queryContextFactory.Create();
            Expression expression = (Expression) ExtractParametersMethod.Invoke(queryCompiler, new object[] {query.Expression, queryContext});
           
            var nodeTypeProvider = (INodeTypeProvider)NodeTypeProviderField.GetValue(queryCompiler);
            var parser = (IQueryParser)CreateQueryParserMethod.Invoke(queryCompiler, new object[] { nodeTypeProvider });
            var queryModel = parser.GetParsedQuery(expression);
            var database = DataBaseField.GetValue(queryCompiler);
            var queryCompilationContextFactory = (IQueryCompilationContextFactory)QueryCompilationContextFactoryField.GetValue(database);
            var queryCompilationContext = queryCompilationContextFactory.Create(false);
            var modelVisitor = (RelationalQueryModelVisitor)queryCompilationContext.CreateQueryModelVisitor();
            modelVisitor.CreateQueryExecutor<TEntity>(queryModel);

            string sql =  modelVisitor.Queries.First().ToString();
            return new SqlWithParameters() {Sql = sql, Parameters = queryContext.ParameterValues?.ToDictionary(k => k.Key, v => v.Value)};
        }
    }

    internal class SqlWithParameters
    {
        public string Sql {get; set;}

        public IDictionary<string, object> Parameters {get; set;}
    }
}