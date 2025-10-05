
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
        HttpClient httpClient,
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

                foreach (PrimaryCatalogItemVariant variant in primaryCatalogItem.PrimaryCatalogItemVariants)
                {
                   
               
                    var endpoint = $"http://localhost:7000/api/remove_upload?url={variant.VariantImageOrigin}";
                    var response = await httpClient.GetAsync(endpoint);
                    response.EnsureSuccessStatusCode();
                    var imageUrl = await response.Content.ReadAsStringAsync();
                    variant.VarianImageEnhanced = imageUrl;


                    variant.PrimaryCatalogItemId = primaryCatalogItem.Id;
                    variant.VarianPriceAdjustments();
                    variant.VarianKeyAdjusted();
                    catalogContext.PrimaryCatalogItemVariants.Add(variant);
                    Console.WriteLine($"{@variant}");
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
