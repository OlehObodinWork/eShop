namespace eShop.Catalog.API.IntegrationEvents.Events
{
    public record CJCatalogItemGrabbedIntegrationEvent(string jsonItem) : IntegrationEvent
    {
    }
}
