using System.Threading.Tasks;

namespace Ataoge.EventBus
{
    public interface IDynamicIntegrationEventHandler
    {
        Task Handle(dynamic eventData);
    }
}