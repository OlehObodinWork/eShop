namespace CJDropship.API.IntegrationEvents.Events
{
    public record CJCatalogItemGrabbedIntegrationEvent(string jsonItem): IntegrationEvent
    {
    }
}
