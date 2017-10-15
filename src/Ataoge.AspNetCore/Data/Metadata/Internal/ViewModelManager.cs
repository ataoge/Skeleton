using System;
using System.Collections.Generic;
using System.ComponentModel;
using Ataoge.Utilities;
using JetBrains.Annotations;
using Newtonsoft.Json;

namespace Ataoge.Data.Metadata.Internal
{
    public class ViewModelManager : IViewModelManager
    {
        public ViewModelManager()
        {

        }

        private readonly IDictionary<Type, IViewModel> _clrTypeMap
            = new Dictionary<Type, IViewModel>();

        private readonly Type UiColumnAttributeType = typeof(UiColumnAttribute);
        private readonly Type UiClassColumnAttributeType = typeof(UiClassColumnAttribute);
        private readonly Type JsonPropertyAttributeType = typeof(JsonPropertyAttribute);
        private readonly Type DisplayNameAttributeType = typeof(DisplayNameAttribute);

        public virtual IViewModel AddViewModel([NotNull] Type clrType)
        {
            var viewModel = new ViewModel();

            var attributes =  (UiClassColumnAttribute[])Attribute.GetCustomAttributes(clrType, UiClassColumnAttributeType);
            foreach(var attr in attributes)
            {
                if (string.IsNullOrEmpty(attr.EntityPropertyName) || string.IsNullOrEmpty(attr.EntityPropertyValueType))
                    continue;

                UiColumnInfo columnInfo = viewModel.CreateColumInfo(attr.EntityPropertyName, attr.QueryKey);
                columnInfo.PropertyValueType = attr.EntityPropertyValueType;
                columnInfo.PropertyValue = attr.EntityPropertyValue;
                columnInfo.DefaultContent = attr.DefaultContent;
                columnInfo.DisplayName = attr.DisplayName;
                columnInfo.Visible = attr.Visible;
                columnInfo.Searchable = attr.Searchable;
                columnInfo.SearchMode = attr.SearchMode;
                columnInfo.ReferField = attr.ReferField;
                columnInfo.ReferCondition = attr.ReferCondition;
                columnInfo.ReferValueFormatMethod = attr.ReferValueFormatMethod;
                columnInfo.Orderable = false;
                columnInfo.Weight = attr.Weight;
                columnInfo.Width = attr.Weight;
                columnInfo.FormatMethod = attr.FormatMethod;
                columnInfo.OperationEvent = attr.OperationEvent;

                

            }

            foreach (var propertyInfo in clrType.GetProperties())
            {
                UiColumnAttribute columnAttribute = (UiColumnAttribute)Attribute.GetCustomAttribute(propertyInfo, UiColumnAttributeType);
                JsonPropertyAttribute  jsonPropertyAttribute = (JsonPropertyAttribute)Attribute.GetCustomAttribute(propertyInfo, JsonPropertyAttributeType);
                DisplayNameAttribute displayNameAttribute = (DisplayNameAttribute)Attribute.GetCustomAttribute(propertyInfo, DisplayNameAttributeType);
                UiColumnInfo columnInfo = viewModel.CreateColumInfo(columnAttribute?.EntityPropertyName ?? propertyInfo.Name, jsonPropertyAttribute?.PropertyName ?? propertyInfo.Name.ToCamelCase());
                columnInfo.DefaultContent = columnAttribute?.DefaultContent;
                columnInfo.DisplayName = columnAttribute?.DisplayName ?? displayNameAttribute?.DisplayName;
                columnInfo.PropertyValueType = columnAttribute?.EntityPropertyValueType ?? Type.GetTypeCode(propertyInfo.PropertyType).ToString();
                columnInfo.PropertyValue = columnAttribute?.EntityPropertyValue;
                columnInfo.Visible = columnAttribute != null ? columnAttribute.Visible : true;
                columnInfo.Orderable = columnAttribute != null ? columnAttribute.Orderable : false;
                columnInfo.Searchable = columnAttribute != null ? columnAttribute.Searchable : false;
                columnInfo.SearchMode = columnAttribute != null ? columnAttribute.SearchMode : FilterMode.NormalAnd;
                columnInfo.ReferField = columnAttribute?.ReferField;
                columnInfo.ReferCondition = columnAttribute?.ReferCondition;
                columnInfo.ReferValueFormatMethod = columnAttribute?.ReferValueFormatMethod;
                columnInfo.Weight = columnAttribute != null ? columnAttribute.Weight : 0;
                columnInfo.Width = columnAttribute != null ? columnAttribute.Width : 0;
                columnInfo.FormatMethod = columnAttribute?.FormatMethod;
                columnInfo.OperationEvent = columnAttribute?.OperationEvent;
            }
            
            if (!_clrTypeMap.ContainsKey(clrType))
                _clrTypeMap[clrType] = viewModel;
                
            return viewModel;
        }

        public virtual IViewModel FindViewModel([NotNull] Type clrType)
        {
            if (_clrTypeMap.ContainsKey(clrType))
                return _clrTypeMap[clrType];
                
            return null;
        }

        public IViewModel GetOrAddViewModel([NotNull] Type clrType) =>
            FindViewModel(clrType) ?? AddViewModel(clrType);

        public IViewModel RemoveViewModel([NotNull] Type clrType)
        {
            IViewModel result = null;
            if (_clrTypeMap.ContainsKey(clrType))
                result = _clrTypeMap[clrType];

            if (_clrTypeMap.Remove(clrType))
                return result;
            return null;
        }

 
    }
}