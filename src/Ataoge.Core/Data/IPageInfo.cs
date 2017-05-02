namespace Ataoge.Data
{
    public interface IPageInfo
    {
        /// <summary>
        /// 当前页的索引，以1开始
        /// </summary>
        int Index
        {
            get;
            //set;
        }

        /// <summary>
        /// 当前页的大小，即每页显示多少行
        /// </summary>
        int Size
        {
            get;
            //set;
        }

        /// <summary>
        /// 总共的记录数
        /// </summary>
        int RecordCount
        {
            get;
            set;
        }

        
        /// <summary>
        /// 排序语句，排序的字段应该设置索引，这样可以大大提高海量数据的检索效率
        /// 如果字段没有建立索引，推荐不要使用该属性，否则会降低效率
        /// </summary>
        string OrderByClause
        {
            get;
            //set;
        }

        /// <summary>
        /// 返回总记录数
        /// </summary>
        bool ReturnRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库的优化方案
        /// </summary>
        int OptimizationFlag
        {
            get;
            //set;
        }
    }
}