using JetBrains.Annotations;

namespace Ataoge.Infrastructure
{
    public class MultiTenancyConfig
    {
        public MultiTenancyConfig()
        {

        }

        protected MultiTenancyConfig([NotNull] MultiTenancyConfig copyFrom)
        {
            _isEnabled = copyFrom.IsEnabled;
        }

        private bool _isEnabled = false; 
       
        /// <summary>
        /// Is multi-tenancy enabled?
        /// Default value: false.
        /// </summary>
        public bool IsEnabled => _isEnabled;

        protected virtual MultiTenancyConfig Clone() => new MultiTenancyConfig(this);


        public virtual MultiTenancyConfig WithIsEnabled(bool enabled)
        {
            var clone = Clone();

            clone._isEnabled = enabled;

            return clone;
        }
    }
}