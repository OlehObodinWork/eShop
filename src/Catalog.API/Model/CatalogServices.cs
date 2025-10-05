using Catalog.API.Services;
using eShop.Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

public class CatalogServices(
    CatalogContext context,
    [FromServices] ICatalogAI catalogAI,
    [FromServices] IPrimaryCatalogAI primaryCatalogAI,
    [FromServices] CJCatalog cJCatalog,
    [FromServices] TokenService tokenService,
    IOptions<CatalogOptions> options,
    ILogger<CatalogServices> logger,
    [FromServices] ICatalogIntegrationEventService eventService)
{
    public CJCatalog CJCatalog { get; } = cJCatalog;
    public CatalogContext Context { get; } = context;

    public TokenService TokenService { get; } = tokenService;
    public ICatalogAI CatalogAI { get; } = catalogAI;

    public IPrimaryCatalogAI PrimaryCatalogAI { get; } = primaryCatalogAI;
    public IOptions<CatalogOptions> Options { get; } = options;
    public ILogger<CatalogServices> Logger { get; } = logger;
    public ICatalogIntegrationEventService EventService { get; } = eventService;
};
