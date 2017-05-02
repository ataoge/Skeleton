using System;

namespace Ataoge.Data
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DbFieldAttribute : Attribute
    {
        public DbFieldAttribute(string name)
        {
            this.Name = name.ToUpper();
        }

        public string Name 
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public string AliasName
        {
            get;
            set;
        }

        public SafDbType SafDbType   {  get; set; } = SafDbType.Default;

        public byte Precision
        {
            get;
            set;
        }

        public byte Scale
        {
            get;
            set;
        }

        public int Flag
        {
            get;
            set;
        }

        public bool NotNull {get; set;} = false;
       
        public object DefaultValue  {get; set;} = null;

        public bool IsPrimaryKey
        {
            get
            {
                return IsFlag(SafFieldType.PrimaryKey);
            }
            set
            {
                SetFlag(SafFieldType.PrimaryKey, value);
            }
        }

        public bool IsUniqeKey
        {
            get
            {
                return IsFlag(SafFieldType.UniqeKey);
            }
            set
            {
                SetFlag(SafFieldType.UniqeKey, value);
            }
        }

        public bool IsForeignKey
        {
            get
            {
                return IsFlag(SafFieldType.ForeignKey);
            }
            set
            {
                SetFlag(SafFieldType.ForeignKey, value);
            }
        }

        public bool IsParentKey
        {
            get
            {
                return IsFlag(SafFieldType.ParentKey);
            }
            set
            {
                SetFlag(SafFieldType.ParentKey, value);
            }
        }

        public bool IsTimeStamp
        {
            get
            {
                return IsFlag(SafFieldType.TimeStamp);
            }
            set
            {
                SetFlag(SafFieldType.TimeStamp, value);
            }
        }

        public bool IsSortIndex
        {
            get
            {
                return IsFlag(SafFieldType.SortIndex);
            }
            set
            {
                SetFlag(SafFieldType.SortIndex, value);
            }
        }

        public bool NeedIndex
        {
            get
            {
                return IsFlag(SafFieldType.NeedIndex);
            }
            set
            {
                SetFlag(SafFieldType.NeedIndex, value);
            }
        }

        private bool IsFlag(SafFieldType safFieldType)
        {
            return safFieldType.IsFlag(this.Flag);
        }

        private void SetFlag(SafFieldType safFieldType, bool value)
        {
            if (value)
            {
                this.Flag = this.Flag | (int)safFieldType;
            }
            else
            {
                this.Flag = this.Flag & (~(int)safFieldType);
            }
        }

       
    }
}