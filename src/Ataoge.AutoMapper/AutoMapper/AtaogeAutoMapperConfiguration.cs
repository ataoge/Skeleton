using System;
using System.Collections.Generic;
using AutoMapper;

namespace Ataoge.AutoMapper
{
    public class AtaogeAutoMapperConfiguration : IAtaogeAutoMapperConfiguration
    {
        public List<Action<IMapperConfigurationExpression>> Configurators { get; }

        public bool UseStaticMapper { get; set; }

        public AtaogeAutoMapperConfiguration()
        {
            UseStaticMapper = true;
            Configurators = new List<Action<IMapperConfigurationExpression>>();
        }
    }
}