using CJDropship.API.IntegrationEvents;
using CJDropship.API.Services.Interfaces;

namespace CJDropship.API.Services
{
    public class CJServices(
        ITokenService tokenService, 
        ILogger<CJServices> logger, 
        CJCatalogService cJCatalogService,
        ICJIntegrationEventService cJIntegrationEventService
        )
    {
        public ITokenService TokenService { get; } = tokenService;

        public ILogger<CJServices> Logger { get; } = logger;

        public CJCatalogService CJCatalogService { get; } = cJCatalogService;

        public ICJIntegrationEventService CJIntegrationEventService { get; } = cJIntegrationEventService;

    }
}
