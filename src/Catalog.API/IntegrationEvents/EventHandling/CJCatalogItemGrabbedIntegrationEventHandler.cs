
using System;
using System.Text.Json;
using Catalog.API.Model;
using Catalog.API.Services;
using eShop.Catalog.API.Model;
using eShop.Catalog.API.Services;

namespace eShop.Catalog.API.IntegrationEvents.EventHandling
{
    public class CJCatalogItemGrabbedIntegrationEventHandler(
        CatalogContext catalogContext,
        IPrimaryCatalogAI primaryCatalogAI,
        ILogger<CJCatalogItemGrabbedIntegrationEventHandler> logger
    ) :IIntegrationEventHandler<CJCatalogItemGrabbedIntegrationEvent>
    {
        public async Task Handle(CJCatalogItemGrabbedIntegrationEvent @event)
        {
            PrimaryCatalogItem primaryCatalogItem = null;
            List<PrimaryCatalogItemVariant> itemVariants = null;
            string data = "";
            string variants = "";

            List<PrimaryCatalogOriginalImages> images = new();
            try
            {

                using (JsonDocument doc = JsonDocument.Parse(@event.jsonItem))
                {
                    JsonElement root = doc.RootElement;

                    // Extract the "data" field
                    if (root.TryGetProperty("data", out JsonElement dataElement))
                    {
                        data = dataElement.GetRawText();
                        dataElement.TryGetProperty("variants", out JsonElement variantsElement);

                        dataElement.TryGetProperty("productImageSet", out JsonElement imagesElement);

                        logger.LogInformation($"Images element - {imagesElement}");
                        foreach (JsonElement element in imagesElement.EnumerateArray())
                        {
                            images.Add(new PrimaryCatalogOriginalImages() { Src = element.ToString() });
                        }
                        variants = variantsElement.GetRawText();
                        logger.LogInformation($"Catalog original images {@images}");

                        logger.LogInformation($"Data - {data}");

                        primaryCatalogItem = JsonSerializer.Deserialize<PrimaryCatalogItem>(data);
                        itemVariants = JsonSerializer.Deserialize<List<PrimaryCatalogItemVariant>>(variants);
                        primaryCatalogItem.PrimaryCatalogItemVariants = itemVariants;
                        primaryCatalogItem.PrimaryCatalogOriginalImages = images;

                    }
                    else
                    {
                        Console.WriteLine("Data field not found.");
                    }
                }

                foreach (var variant in primaryCatalogItem.PrimaryCatalogItemVariants)
                {
                    variant.PrimaryCatalogItemId = primaryCatalogItem.Id;
                    variant.VarianPriceAdjustments();
                    variant.VarianKeyAdjusted();
                    catalogContext.PrimaryCatalogItemVariants.Add(variant);
                }

                foreach (var image in primaryCatalogItem.PrimaryCatalogOriginalImages)
                {
                    image.PrimaryCatalogItemId = primaryCatalogItem.Id;
                    catalogContext.PrimaryCatalogOriginalImages.Add(image);
                }

                primaryCatalogItem.Embedding = await primaryCatalogAI.GetEmbeddingAsync(primaryCatalogItem);

                catalogContext.PrimaryCatalogItems.Add(primaryCatalogItem);
               
                await catalogContext.SaveChangesAsync();

            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error processing CJCatalogIemGrabbedIntegrationEventHandler");
            }

        }
    }
}
