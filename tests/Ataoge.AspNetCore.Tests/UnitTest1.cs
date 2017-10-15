using System;
using Ataoge.Data;
using Ataoge.Data.Metadata;
using Ataoge.Data.Metadata.Internal;
using Xunit;

namespace Ataoge.AspNetCore.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            IViewModelManager viewModelManager = new ViewModelManager();
            var viewModel= viewModelManager.GetOrAddViewModel<TestDto>();
        }
    }

    [UiClassColumn(DisplayName = "ABD", EntityPropertyName = "Abc.Inde", EntityPropertyValueType = "Int32", Searchable = true)]
    [UiClassColumn(DisplayName = "CBD", EntityPropertyName = "BBC", EntityPropertyValueType = "String")]
    public class TestDto
    {
        [UiColumnAttribute(DisplayName = "KEY")]
        public int Key {get;set;}

        [UiColumnAttribute(DisplayName = "ABC", EntityPropertyName="AValue")]
        public string Value {get; set;}
    }
}
