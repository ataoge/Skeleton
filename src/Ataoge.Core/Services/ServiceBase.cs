using System;
using System.Linq;
using System.Collections.Generic;
using System.Linq.Expressions;
using Ataoge.Data;
using Ataoge.Data.Metadata;
using Ataoge.Linq;
using Ataoge.Repositories;

namespace Ataoge.Services
{
    public abstract class ServiceBase : IService
    {
        protected ServiceBase(IServiceContext serviceContext)
        {
            this.ServiceContext = serviceContext;
        }

        public IServiceContext ServiceContext { get;}

        
    }

    public abstract class EntityServiceBase : ServiceBase
    {
        protected EntityServiceBase(IServiceContext serviceContext, IViewModelManager viewModelManager, IRepositoryManager repositoryManager) : base (serviceContext)
        {
            this.ViewModelManager = viewModelManager;
            this.RepositoryManager = repositoryManager;
        }

        protected IViewModelManager ViewModelManager {get;}
        protected IRepositoryManager RepositoryManager {get;}

        ///<summary>
        ///根据模型生成 Filter 表达式
        ///</summary>
        protected List<Expression<Func<TEntity, bool>>> BuildFilterInfo<TEntity>(IViewModel vm, IFilterInfo filterInfo)
        {
            //将条件按照列组合起来
            if (filterInfo.Filters != null && filterInfo.Filters.Count() > 0)
            {
                return BuildWhereExpression<TEntity>(vm, filterInfo.Filters);
            }
            return null;
        }

        protected List<Expression<Func<TEntity, bool>>> BuildWhereExpression<TEntity>(IViewModel viewModel, IList<string> where)
        {
            //将条件按照列组合起来
            Dictionary<UiColumnInfo, List<Condition>> dictConditions = new Dictionary<UiColumnInfo, List<Condition>>();

            foreach (var filter in where)
            {
                var ss = filter.Split(' ');
                if (ss.Length != 3)
                    continue;
                var columnInfo = viewModel.GetColumnInfo(ss[0]);
                var condition = new Condition() { Key = ss[1], Value = ss[2] };
                if (columnInfo != null)
                {
                    if (dictConditions.ContainsKey(columnInfo))
                    {
                        List<Condition> conditions = dictConditions[columnInfo];
                        conditions.Add(condition);
                    }
                    else
                    {
                        List<Condition> conditions = new List<Condition>();
                        conditions.Add(condition);
                        dictConditions[columnInfo] = conditions;
                    }
                }
            }


            List<Expression<Func<TEntity, bool>>> expressions = new List<Expression<Func<TEntity, bool>>>();

            foreach (var columnCondition in dictConditions)
            {
                ParameterExpression parameterExpression = ExpressionHelper.BuildParameterExpression(typeof(TEntity));
                UiColumnInfo columnInfo = columnCondition.Key;
                Expression<Func<TEntity, bool>> prediateExpression = BuildColumnConditionExpression(parameterExpression, typeof(TEntity), columnInfo, columnCondition.Value)
                                                                        as Expression<Func<TEntity, bool>>;
                if (prediateExpression != null)
                    expressions.Add(prediateExpression);

            }
            return expressions;
        }

        protected virtual Expression BuildColumnConditionExpression(ParameterExpression parameterExpression, Type entityType, UiColumnInfo columnInfo, List<Condition> conditions)
        {
            return ExpressionHelper.BuildColumnConditionExpression(parameterExpression, entityType, columnInfo.PropertyName, columnInfo.PropertyValueType, columnInfo.SearchMode, conditions, false);
        }

       protected IEnumerable<UiColumnInfo> GetColumnInfo<TEntity>()
       {
            var viewModel = ViewModelManager.GetOrAddViewModel<TEntity>();
            return viewModel.GetSortedColumnInfos();
       }

      
    }

    
}