using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            IServiceCollection services = new ServiceCollection();

            
            services.AddDbContext<TestDbContext>(options => {
                options.UseSqlite("Data Source=test.db");
                options.AddEntityTypeConfigurations();
            });
            services.AddSingleton<IModelCustomizer, EntityTypeConfigurationModelCustomizer>();

         
          

            IServiceProvider sp = services.BuildServiceProvider();
            var bb = "bb";
            using(var dbContext = sp.GetService<TestDbContext>())
            {
                var query = dbContext.Set<SequencesRule>().Where(BuildLamdaExpression<SequencesRule>("PatternName eq aaa", bb)).OrderBy(GetPropertyExpression<SequencesRule>("PatternName"));
                string aa = query.ToSql(true);
                var result =query.ToList();

            }

        }

        private Expression< Func<TEntity, bool>> BuildLamdaExpression<TEntity>(string abc, string bb)
        {
            string[] aa = abc.Split(" ");
            var param = Expression.Parameter(typeof(TEntity), "t");
            var left = Expression.Property(param, aa[0].Trim());
            var right =  Expression.Constant(bb);//aa[2].Trim());
            
            Expression body;
            switch(aa[1].ToLower())
            {
                 case "gt":
                    body = Expression.GreaterThan(left, right); 
                    break;
                case "eq":
                default:
                    body = Expression.Equal(left, right);
                    break;

            }   
           
            var lambda = Expression.Lambda<Func<TEntity, bool>>(body, param);
            return lambda;
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
