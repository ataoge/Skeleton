namespace Ataoge.Data
{
    public interface ITreeQueryInfo
    {
       /// <summary>
        /// 父节点名称
        /// </summary>
        string ParentFieldName
        {
            get;
        }

        /// <summary>
        /// 子节点名称
        /// </summary>
        string FieldName
        {
            get;
        }

        /// <summary>
        /// 是否由子节点向上查询查父节点
        /// </summary>
        bool UpSearch
        {
            get;
        }

        /// <summary>
        /// 设置查询起始节点的Sql语句
        /// </summary>
        string StartWithSql
        {
            get;
        }

        SafParameter[] StartWithParameters
        {
            get;
        }

        string SiblingsOrderBy
        {
            get;
        }

        string PseudoColumns
        {
            get;
        }

        /// <summary>
        /// 伪列查询条件或者附加在树查询上的条件
        /// 例如：Oracle的Level = 1或者 不显示本级，显示下一级列表 id <> 'xxx'
        /// </summary>
        string PseudoWhere
        {
            get;
        } 
    }
}