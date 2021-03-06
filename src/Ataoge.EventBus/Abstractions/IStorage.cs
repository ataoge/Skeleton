using System.Threading;
using System.Threading.Tasks;
using Ataoge.EventBus.Monitoring;

namespace Ataoge.EventBus
{
    /// <summary>
    /// Represents a persisted storage.
    /// </summary>
    public interface IStorage
    {
        /// <summary>
        /// Initializes the storage. For example, making sure a database is created and migrations are applied.
        /// </summary>
        Task InitializeAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Provider the dashboard metric api.
        /// </summary>
        IMonitoringApi GetMonitoringApi();

        /// <summary>
        /// Storage connection of database operate.
        /// </summary>
        IStorageConnection GetConnection();
    }
}