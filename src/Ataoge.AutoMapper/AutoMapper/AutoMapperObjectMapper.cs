using System;
using Ataoge.ObjectMapping;
using AutoMapper;
using IObjectMapper = Ataoge.ObjectMapping.IObjectMapper;

namespace Ataoge.AutoMapper
{
    public class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestination Map<TDestination>(object source)
        {
             return _mapper.Map<TDestination>(source);
        }

        public TDestination Map<TDestination>(object source, Action<IMapperContext> operationAction = null)
        {
            if (operationAction == null)
            {
               return Map<TDestination>(source);
            }
            var ctx = new AtaogeMapperContext();
            return _mapper.Map<TDestination>(source, opts => {
                ctx.Items = opts.Items;
                operationAction(ctx);
            });
        }

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination, Action<IMapperContext> operationAction = null)
        {
            if (operationAction == null)
            {
                return Map(source, destination);
            }
            var ctx = new AtaogeMapperContext();
            return _mapper.Map(source, destination, opts => {
                ctx.Items = opts.Items;
                operationAction(ctx);
            });
        }

      

        public TDestination Map<TSource, TDestination>(TSource source, TDestination destination)
        {
            return _mapper.Map(source, destination);
        }
    }
}