namespace Ataoge.Data
{
    public class PageInfo : IPageInfo
    {

        private int index;
        private int size;
        private int recordCount;
        private string orderByClause;

        public PageInfo() : this(0, 20)
        {
            
        }

        public PageInfo(int index, int size)
        {
            this.index = index;
            this.size = size;
            if (index > 0)
                ReturnRecordCount = true;
        }

         /// <summary>
        /// 获取总页数
        /// </summary>
        /// <returns>总页数</returns>
        public int GetPageCount()
        {
            if ((this.recordCount % size) > 0)
            {
                return this.recordCount / this.size + 1;
            }
            else
            {
                return this.recordCount / this.size;
            }
        }


        #region IPageInfo 成员

        /// <summary>
        /// 当前页的索引，以1开始
        /// </summary>
        public int Index
        {
            get
            {
                return this.index;
            }
            set
            {
                this.index = value;
            }
        }

        /// <summary>
        /// 当前页的大小，即每页显示多少行
        /// </summary>
        public int Size
        {
            get
            {
                return this.size;
            }
            set
            {
                this.size = value;
            }
        }

        /// <summary>
        /// 总共的记录数
        /// </summary>
        public int RecordCount
        {
            get
            {
                return this.recordCount;
            }
            set
            {
                this.recordCount = value;
            }
        }

        /// <summary>
        /// 用来排序的字段，需要用到索引
        /// </summary>
        public virtual string OrderByClause
        {
            get
            {
                return this.orderByClause;
            }
            set
            {
                this.orderByClause = value;
            }
        }

       

        public bool ReturnRecordCount
        {
            get;
            set;
        }

        /// <summary>
        /// 数据库的优化方案
        /// </summary>
        /// <value>
        /// The optimization flag.
        /// </value>
        public int OptimizationFlag
        {
            get;

            set;
        }

        #endregion


    }
}