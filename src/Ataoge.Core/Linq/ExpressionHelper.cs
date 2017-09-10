using System;
using System.Linq.Expressions;

namespace Ataoge.Linq
{
    public static class ExpressionHelper
    {
        
        public static Expression<Func<TEntity, object>> BuildPropertyExpression<TEntity>(string propertyName) 
        {
            //1.创建表达式参数（指定参数或变量的类型:p）
            ParameterExpression param = Expression.Parameter(typeof(TEntity));  
            //2.构建表达式体(类型包含指定的属性:p.Name)  
            MemberExpression body = Expression.Property(param, propertyName);  
            //3.根据参数和表达式体构造一个lambda表达式  
            return Expression.Lambda<Func<TEntity, object>>(body, param); 
        }

        public static Expression<Func<TEntity, bool>> BuildPrediateExpression<TEntity, TValue>(string propertyName, string op, TValue value)
        {
            ParameterExpression param = Expression.Parameter(typeof(TEntity), "t"); 
        
            var left = Expression.Property(param, propertyName);
            var right = value.GetVarName(v => value);

            Expression body;
            switch(op.ToLower())
            {
                 case "gt":
                    body = Expression.GreaterThan(left, right); 
                    break;
                case "eq":
                default:
                    body = Expression.Equal(left, right);
                    break;
            }   
            return Expression.Lambda<Func<TEntity, bool>>(body, param);
        }

        private static Expression GetVarName<T>(Expression<Func<T, T>> exp)
        {
            return exp.Body;
        }

        private static Expression GetVarName<T>(this T t, Expression<Func<T, T>> exp)
        {
            return exp.Body;
        }



    }
}