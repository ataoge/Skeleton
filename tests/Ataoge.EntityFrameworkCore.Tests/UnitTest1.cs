using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Ataoge.Data.Entities;
using Ataoge.EntityFrameworkCore.ModelConfiguration.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {

        }

        [Fact]
        public void TestRepostory()
        {
            var bb = "bb";
            TestExpr<SequencesRule>(t => t.PatternName == bb);
            IServiceCollection services = new ServiceCollection();

            
            services.AddDbContext<TestDbContext>(options => {
                options.UseSqlite("Data Source=test.db");
                options.AddEntityTypeConfigurations();
            });
            services.AddSingleton<IModelCustomizer, EntityTypeConfigurationModelCustomizer>();

            Expression<Func<TestA, object>> cd = t => t.TestClass.Id;
           
            IServiceProvider sp = services.BuildServiceProvider();
     
            using(var dbContext = sp.GetService<TestDbContext>())
            {
               
                Expression<Func<SequencesRule, bool>>  aaa = BuildLamdaExpression<SequencesRule>("PatternName eq aaa", bb) as Expression<Func<SequencesRule, bool>> ;
                var query = dbContext.Set<SequencesRule>().Where(aaa).OrderBy(GetPropertyExpression<SequencesRule>("PatternName"));
                string aa = query.ToSql(true);
                var result =query.ToList();

            }

        }

       

        private void TestExpr<TEntity>(Expression< Func<TEntity, bool>> bbb)
        {
            
        }

        private Expression BuildLamdaExpression<TEntity>(string abc, string bb)
        {
            string[] aa = abc.Split(" ");
            var param = Expression.Parameter(typeof(TEntity), "t");
            var left = Expression.Property(param, aa[0].Trim());
            
            string cc = bb;
            var ddd = GetVarName<string>(t => cc);
            var right =  Expression.Variable(typeof(string), "cc");//.Constant(bb);//aa[2].Trim());
            var asn = Expression.Assign(right, Expression.Constant(bb));
            
            var mm = typeof(string).GetTypeInfo().GetMembers();
            Expression body;
            switch(aa[1].ToLower())
            {
                 case "gt":
                    body = Expression.GreaterThan(left, right); 
                    break;
                case "eq":
                default:
                    body = Expression.Equal(left, ddd);
                    break;

            }   
           
            var lambda = Expression.Lambda(body, param);
            return lambda;
        }

        public static Expression GetVarName<T>(System.Linq.Expressions.Expression<Func<T, T>> exp)
        {
            return exp.Body;
        }

        public Expression<Func<TEntity, object>> GetPropertyExpression<TEntity>(string propertyName) 
        {
             ParameterExpression param = Expression.Parameter(typeof(TEntity));  
            //2.构建表达式体(类型包含指定的属性:p.Name)  
            MemberExpression body = Expression.Property(param, propertyName);  
            //3.根据参数和表达式体构造一个lambda表达式  
            return Expression.Lambda<Func<TEntity, object>>(body, param); 
        }

        public LambdaExpression GetLambdaExpression<TEntity>(string propertyName)  
        {  
            //1.创建表达式参数（指定参数或变量的类型:p）  
            ParameterExpression param = Expression.Parameter(typeof(TEntity));  
            //2.构建表达式体(类型包含指定的属性:p.Name)  
            MemberExpression body = Expression.Property(param, propertyName);  
            //3.根据参数和表达式体构造一个lambda表达式  
            return Expression.Lambda(body, param);  
        }  
    }
}
