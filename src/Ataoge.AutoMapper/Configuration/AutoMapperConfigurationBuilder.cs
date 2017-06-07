using System;
using System.Reflection;
using Ataoge.Infrastructure;
using AutoMapper;

namespace Ataoge.Configuration
{
    public class AutoMapperConfigurationBuilder : SkeletonOptionsExtensionBuilder<AutoMapperConfigurationBuilder, AutoMapperConfiguration>
    {
        public AutoMapperConfigurationBuilder(SkeletonOptionsBuilder optionsBuilder) : base(optionsBuilder)
        {
        }


        public virtual AutoMapperConfigurationBuilder DefaultMapper(bool staticMapper)
            => WithOption(e => e.WithStaticMapper(staticMapper));

        public virtual AutoMapperConfigurationBuilder AddProfileAssembly(params Assembly[] assemblies)
        {
            IProfileProvider provider = new AssemblyProfileProvider(assemblies);
            return AddProfileProvider(provider);
        }

        public virtual AutoMapperConfigurationBuilder AddProfileProvider(IProfileProvider provider)
            => WithOption(e => e.WithProfileProvider(provider));

        public virtual AutoMapperConfigurationBuilder AddMapperConfigurationExpression(Action<IMapperConfigurationExpression> configAction)
            => WithOption(e => e.WithMapperConfigurationExpression(configAction));
     
    }
}