using Ataoge.EventBus;
using Ataoge.EventBus.InMemoryStorage;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemoryConfigOptionsExtensions
    {
        public static ConfigOptions UseInMemoryStorage(this ConfigOptions options)
        {
            options.RegisterExtension(new InMemoryConfigOptionsExtension());
                                          
            return options;
        } 
    }
} 