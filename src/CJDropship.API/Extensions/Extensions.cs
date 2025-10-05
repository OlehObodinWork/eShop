using CJDropship.API.IntegrationEvents;
using CJDropship.API.Services;
using CJDropship.API.Services.Interfaces;

namespace CJDropship.API.Extensions
{
    public static class Extensions
    {
        public static void AddApplicationServices(this IHostApplicationBuilder builder)
        {

            builder.Services.AddScoped<CJCatalogService>();
            builder.Services.AddSingleton<ITokenService, CJTokenService>();


            builder.AddRabbitMqEventBus("eventbus");
            builder.Services.AddTransient<ICJIntegrationEventService, CJIntegrationEventService>();

            builder.Services.AddScoped<CJServices>();
        }
    }

}
