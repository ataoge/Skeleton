using System;
using System.Collections.Generic;
using Ataoge.Infrastructure;
using AutoMapper;
using JetBrains.Annotations;

namespace Ataoge.Configuration
{
    public class AutoMapperConfiguration : ISkeletonOptionsExtension
    {
        public AutoMapperConfiguration()
        {

        }

        protected AutoMapperConfiguration(AutoMapperConfiguration copyFrom)
        {
            this._staticMapper = copyFrom.StaticMapper;
            this._provider = copyFrom.Provider;

            if (copyFrom._configurators != null)
            {
                this._configurators = new List<Action<IMapperConfigurationExpression>>(copyFrom._configurators);
            }
        }
        
        protected virtual AutoMapperConfiguration Clone()
            => new AutoMapperConfiguration(this);


        private bool _staticMapper = true;
        
        public virtual bool StaticMapper => _staticMapper;

        public virtual AutoMapperConfiguration WithStaticMapper(bool staticMapper = true)
        {
            //Check.NotNull(connectionString, nameof(connectionString));

            var clone = Clone();

            clone._staticMapper = staticMapper;

            return clone;
        }

        private IProfileProvider _provider;
        
        public IProfileProvider Provider => _provider;

        public virtual AutoMapperConfiguration WithProfileProvider([NotNull] IProfileProvider provider)
        {
            Check .NotNull(provider, nameof(provider));

            var clone = Clone();

            clone._provider = provider;

            return clone;

        }

        private List<Action<IMapperConfigurationExpression>> _configurators;

        public List<Action<IMapperConfigurationExpression>> Configurators => _configurators;

        public virtual AutoMapperConfiguration WithMapperConfigurationExpression([NotNull] Action<IMapperConfigurationExpression> configAction)
        {
            var clone = Clone();

            if (clone._configurators == null)
            {
                clone._configurators = new List<Action<IMapperConfigurationExpression>>();
            }

            clone._configurators.Add(configAction);

            return clone;
        }

        public void Validate(ISkeletonOptions options)
        {
            
        }
    }
}