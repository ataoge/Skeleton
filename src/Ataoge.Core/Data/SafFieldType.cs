using System;

namespace Ataoge.Data
{
    [Flags]
    public enum SafFieldType : int
    {
        /// <summary>
        /// 主键字段
        /// </summary>
        PrimaryKey = 1,

        /// <summary>
        /// 唯一型字段
        /// </summary>
        UniqueKey = 2,

        /// <summary>
        ///外键字段 
        /// </summary>
        ForeignKey = 4,

        /// <summary>
        /// 父键字段
        /// </summary>
        ParentKey = 8,

        /// <summary>
        /// 时间戳字段
        /// </summary>
        TimeStamp = 16,

        /// <summary>
        /// 专用排序字段
        /// </summary>
        SortIndex = 32,

        /// <summary>
        /// 专用排序字段
        /// </summary>
        NeedIndex = 64
        
    }
}