using CJDropship.API.IntegrationEvents.Events;
using CJDropship.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;


namespace CJDropship.API.Apis
{
    public static class CJDropshipApi
    {
        public static IEndpointRouteBuilder MapCJApiV1(this IEndpointRouteBuilder app)
        {

            var vApi = app.NewVersionedApi("CJDropship");
            var api = vApi.MapGroup("api/cj-catalog").HasApiVersion(1, 0).HasApiVersion(2, 0);
            var v1 = vApi.MapGroup("api/cj-catalog").HasApiVersion(1, 0);
            var v2 = vApi.MapGroup("api/cj-catalog").HasApiVersion(2, 0);

            api.MapGet("items/sync", SyncItem);

            return app;
        }


        public static async Task<Results<Created, Ok>> SyncItem([FromServices] CJServices services, string SKU)
        {
            var url = $"https://developers.cjdropshipping.com/api2.0/v1/product/query?productSku={SKU}";

            var token = "";
            if (services.TokenService.token != null)
            {
                token = services.TokenService.token;
            }
            else
            {
                token = await services.TokenService.GetTokenAsync();
            }

            services.Logger.LogInformation(token);

            var result = await services.CJCatalogService.GetCatalogItemAsync(url, services.Logger, token!);

            var evt = new CJCatalogItemGrabbedIntegrationEvent(result);
            await services.CJIntegrationEventService.PublishThroughEventBusAsync(evt);
            return TypedResults.Created();
        }

    }
}
