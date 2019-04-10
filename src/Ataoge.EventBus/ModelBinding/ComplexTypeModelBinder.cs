using System;
using System.Reflection;
using System.Threading.Tasks;
using Ataoge.EventBus.Abstractions;

namespace Ataoge.EventBus.ModelBinding
{
    internal class ComplexTypeModelBinder : IModelBinder
    {
        private readonly ParameterInfo _parameterInfo;
        private readonly IContentSerializer _serializer;

        public ComplexTypeModelBinder(ParameterInfo parameterInfo, IContentSerializer contentSerializer)
        {
            _parameterInfo = parameterInfo;
            _serializer = contentSerializer;
        }

        public Task<ModelBindingResult> BindModelAsync(string content)
        {
            try
            {
                var type = _parameterInfo.ParameterType;

                var value = _serializer.DeSerialize(content, type);

                return Task.FromResult(ModelBindingResult.Success(value));
            }
            catch (Exception)
            {
                return Task.FromResult(ModelBindingResult.Failed());
            }
        }
    }
}