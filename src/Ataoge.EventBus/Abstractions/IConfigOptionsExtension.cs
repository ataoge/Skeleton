using Microsoft.Extensions.DependencyInjection;

namespace Ataoge.EventBus
{
    /// <summary>
    /// Cap options extension
    /// </summary>
    public interface IConfigOptionsExtension
    {
        /// <summary>
        /// Registered child service.
        /// </summary>
        /// <param name="services">add service to the <see cref="IServiceCollection" /></param>
        void AddServices(IServiceCollection services);
    }
}