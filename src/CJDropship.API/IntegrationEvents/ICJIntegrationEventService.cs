using eShop.EventBus.Events;
namespace CJDropship.API.IntegrationEvents

{
    public interface ICJIntegrationEventService
    {
        void SaveEventAsync(IntegrationEvent evt);
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}


