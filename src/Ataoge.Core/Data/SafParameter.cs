using System;
using static Ataoge.Data.SafDbTypeHelper;

namespace Ataoge.Data
{
    public class SafParameter
    {
        private SafDbType _safDbType;
        private int _direction = 1;

        private bool _isNullable;
        private string _parameterName;
        private int _size;
        private string _sourceColumn;
        private bool _sourceColumnNullMapping;

        private object _value;

        private byte _precision = 0;
        private byte _scale = 0;

        public SafParameter()
        {
            this._safDbType = SafDbType.String;
            this._direction = 1;
            this._isNullable = false;
            this._size = 0;
            this._precision = 0;
            this._scale = 0;
            
        }

        public SafParameter(string parameterName, object value) : this()
        {
            this._parameterName = parameterName;
            this.Value = value;
            this._safDbType = GetSafDbTypeForObject(value);
        }

        public SafParameter(string parameterName, SafDbType safDbType, object value) :this()
        {
            this._parameterName = parameterName;
            this._safDbType = safDbType;
            this.Value = value;
        }

        public SafParameter(string parameterName, SafDbType safDbType, int direction, int size, bool isNullable, byte precision, byte scale, string sourceColumn, bool sourceColumnNullMapping, object value)
        {
            this._parameterName = parameterName;
            this._safDbType = safDbType;
            this._direction = direction;
            this._size = size;
            this._isNullable = isNullable;
            this._precision = precision;
            this._scale = scale;
            this._sourceColumn = sourceColumn;
            this._sourceColumnNullMapping = sourceColumnNullMapping;
            this.Value = value;
        }

        

        public virtual SafDbType SafDbType
        {
            get
            {
                return this._safDbType;
            }
            set
            {
                this.SafDbType = value;
            }
        }

        public virtual int Direction
        {
            get {return this._direction;}
            set { this._direction = value;}
        }

         public virtual bool IsNullable
        {
            get
            {
                return this._isNullable;
            }
            set
            {
                this._isNullable = value;
            }
        }

        public virtual string ParameterName
        {
            get
            {
                string str = this._parameterName;
                if (str == null)
                {
                    return "";
                }
                return str;

            }
            set
            {
                if (this._parameterName != value)
                {
                    this._parameterName = value;
                }
            }
        }

        public virtual void ResetDbType()
        {
            this._safDbType = SafDbType.String;
        }

        public virtual int Size
        {
            get
            {
                int num = this._size;
                if (num == 0)
                {
                    num = this.ValueSize(this.Value);
                }
                return num;

            }
            set
            {
                if (this._size != value)
                {
                    this._size = value;
                }
    
            }
        }

        private int ValueSize(object value)
        {
            //daydream：添加string赋值并设置size值
            if (value != null && value.GetType().Equals(typeof(string)))
                return ((string)value).Length;
            return 0;
        }

        public virtual string SourceColumn
        {
            get
            {
                return this._sourceColumn;
            }
            set
            {
                this._sourceColumn = value;
            }
        }

        public virtual bool SourceColumnNullMapping
        {
            get
            {
                return this._sourceColumnNullMapping;
            }
            set
            {
                this._sourceColumnNullMapping = value;
            }
        }

        public virtual object Value
        {
            get
            {
                return this._value;
            }
            set
            {
                if (value == null)
                {
                    this._isNullable  = true;
                }
                else 
                {
                    Type valueType = value.GetType();
                    if (valueType == typeof(char) || valueType == typeof(char[]))
                    {
                        this._value= value.ToString();
                        return;
                    }
                }
                this._value = value;
            }
        }

        public virtual byte Precision
        {
            get
            {
                return this._precision;
            }
            set
            {
                this._precision = value;
            }
        }

        public virtual byte Scale
        {
            get
            {
                return this._scale;
            }
            set
            {
                this._scale = value;
            }
        }


    }
}