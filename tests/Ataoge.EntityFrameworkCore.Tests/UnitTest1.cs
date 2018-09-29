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
using Microsoft.Extensions.Logging;
using Xunit;

namespace Ataoge.EntityFrameworkCore.Tests
{
    public class UnitTest1
    {
        public UnitTest1()
        {

        }

        [Fact]
        public void TestRawSql()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            
            services.AddDbContext<TestDbContext>(options => {
                options.UseSqlite("Data Source=test.db");
                options.AddEntityTypeConfigurations();
                
            });

            IServiceProvider sp = services.BuildServiceProvider();
            var loggerFactory = sp.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Debug);
            //loggerFactory.AddDebug();

     
            using(var dbContext = sp.GetService<TestDbContext>())
            {
                var aa = dbContext.Set<Test>().Where(t => t.Id == 1).ToArray();
                var bb = dbContext.Set<Test>().FromSql("select * from test1").ToArray();
                var cc = dbContext.Set<Test>().Select(t => t.Name).FromSql("select name from test1").ToArray();
                var ee = dbContext.Set<Test>().FromSql("select * from test1").Where(t => t.Id == 1).ToArray();
                //error
                //var dd =  dbContext.Set<Test>().FromSql("select name from test1").ToArray();//.Select(t => t.Name).ToArray();
                
            }
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
                var query = dbContext.Set<SequencesRule>();//.Where(aaa).OrderBy(GetPropertyExpression<SequencesRule>("PatternName"));
                int vv = 10;
                var exss = new string[] {"bb", "cc"};//
                var aa = query.Where(t => t.MaxValue > vv && exss.Contains(t.PatternName)).ToSql(true);
                var result =query.ToList();

            }

        }

        [Fact]
        public void TestSqlQuery()
        {
            IServiceCollection services = new ServiceCollection();

            
            services.AddDbContext<TestDbContext>(options => {
                options.UseSqlite("Data Source=test.db");
                options.AddEntityTypeConfigurations();
            });

            services.AddScoped<IScope, MyScope>();

            services.AddSingleton<ISingleton>(p => {
                var scope = p.GetRequiredService<IScope>();
                return new Singleton(scope);
            });

            IServiceProvider sp = services.BuildServiceProvider();

            var ss = sp.GetRequiredService<ISingleton>();
            var sc = sp.GetRequiredService<IScope>();
            var bb = sp.GetRequiredService<ISingleton>();
     
            IEnumerable<TestEntity> aa = null;
            using(var dbContext = sp.GetService<TestDbContext>())
            {
                aa = dbContext.Database.GetEntityFromSqlQuery<TestEntity, int>(() => new TestEntity(), "select t.Id, T.Name from Test t");
            }

            var testEntity = aa.ToList();

        }

        [Fact]
        public void TestAuthSQL()
        {
             IServiceCollection services = new ServiceCollection();

            services.AddSingleton<ILoggerFactory, LoggerFactory>();
            
            services.AddDbContext<TestDbContext>(options => {
                options.UseSqlite("Data Source=test.db");
                options.AddEntityTypeConfigurations();
                
            });

            IServiceProvider sp = services.BuildServiceProvider();
            var loggerFactory = sp.GetService<ILoggerFactory>();
            loggerFactory.AddConsole(LogLevel.Debug);
            //loggerFactory.AddDebug();

     
            using(var dbContext = sp.GetService<TestDbContext>())
            {
                IQueryable<Test> query = dbContext.Set<Test>();
                //var aa = dbContext.ResourcePermissionAssign;
                //query = AddAuth(query, aa, 1);
                int[] ids = new int[]{1,2};
                query = query.Where(t => t.ResourcePermissionAssigns.Any(d => ids.Contains(d.RoleId) && d.Operation > 0)).Where(t => !t.ResourcePermissionAssigns.Any(d => ids.Contains(d.RoleId) && d.Operation > 0));
                var bb = query.Take(5).ToArray();
            }

        }

        private IQueryable<Test> AddAuth(IQueryable<Test> query, IQueryable<ResourcePermissionAssign> permissionAssigns, params int[] roles)
        {
            var notids = permissionAssigns.Where(t => roles.Contains(t.RoleId) && t.ResourceType =="test" && t.IsRefused > 0).Select(t => t.ResourceId);
            var ids = permissionAssigns.Where(t => roles.Contains(t.RoleId) && t.ResourceType =="test").Select(t => t.ResourceId);
            return query.Where(t => !notids.Contains(t.Id) &&  ids.Contains(t.Id));
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

    public interface ISingleton
    {
        string Name {get; set;}
    }

    public interface IScope
    {
        string Name {get; set;}
    }

    public class MyScope : IScope
    {
        public string Name {get; set;} = Guid.NewGuid().ToString();
    }

    public class Singleton : ISingleton
    {
        public Singleton(IScope scope)
        {
            Name = scope.Name;
        }

        public string Name {get; set;} 

    }
    
}
