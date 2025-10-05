using System.Text.Json;
using Catalog.API.Model;
using Microsoft.Extensions.Logging;

namespace eShop.Catalog.API.Services
{

    public class CJCatalog
    {
        public async Task<PrimaryItemWithVariants> GetCatalogItemAsync(string url, ILogger logger, string token)
        {
            HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("CJ-Access-Token", token);

            using HttpResponseMessage response = await httpClient.GetAsync(url);
            PrimaryCatalogItem result = null;
            List<PrimaryCatalogItemVariant> itemVariants = null;
            string data = "";
            string variants = "";
            List<PrimaryCatalogOriginalImages> images = new();
            try
            {
                response.EnsureSuccessStatusCode();
                var jsonResponse = await response.Content.ReadAsStringAsync();
                logger.LogInformation($"Response from api {jsonResponse}");

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
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

                        //foreach (JsonElement element in variants.EnumerateArray())
                        //{
                        //    string variantImage = element.GetProperty("variantImage").GetString();

                        //    Console.WriteLine($"Name: {variantImage}"); 
                        //}
                        //Console.WriteLine($"Product variants: {variants}");
                        result = JsonSerializer.Deserialize<PrimaryCatalogItem>(data);
                        itemVariants = JsonSerializer.Deserialize<List<PrimaryCatalogItemVariant>>(variants);
                    }
                    else
                    {
                        Console.WriteLine("Data field not found.");
                    }
                }



            }
            catch (HttpRequestException)
            {

            }

            return new PrimaryItemWithVariants { Item = result, Variants = itemVariants, Images = images };

        }
    }

    public class PrimaryItemWithVariants
    {
        public PrimaryCatalogItem Item;

        public List<PrimaryCatalogItemVariant> Variants;

        public List<PrimaryCatalogOriginalImages> Images;
    }
}
