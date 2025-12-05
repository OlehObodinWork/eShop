using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using eShop.WebAppComponents.Catalog;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using static System.Net.WebRequestMethods;
namespace eShop.WebApp.Services
{
    public class GetPrimaryCatalogItem
    {
        public  IConfiguration _configuration;
        public  HttpClient _http;
        public PrimaryCatalogItemDto? CatalogItem { get; private set; }

        public List<PrimaryFeatureValueDto>? CatalogItemFeatures { get; private set;  }
        private IEnumerable<IGrouping<string, CatalogItemVariantDto>>? CatalogItemVariants { get; set; }

        public GetPrimaryCatalogItem(HttpClient http, IConfiguration configuration)
        {
            _http = http;
            _configuration = configuration;
        }

        public async Task<(PrimaryCatalogItemDto?, IEnumerable<IGrouping<string, CatalogItemVariantDto>>?)> GetPrimaryCatalogItems(System.Uri baseUrl)
        {

            string lastSegment = baseUrl.Segments.Last().TrimEnd('/');



            var url = $"{_configuration["Endpoints:CatalogAPI"]}/api/primary-catalog/items/{lastSegment}?api-version=1";

            var response = await _http.GetFromJsonAsync<PrimaryCatalogItemDto>(url);


            CatalogItem = response;


            CatalogItemVariants = CatalogItem?.PrimaryCatalogItemVariants?.Where(item => item.variantKey != null).GroupBy(item => item.variantKey?.Split("-")[0] ?? "Unknown");

            return (CatalogItem, CatalogItemVariants);
            
        }

        public async Task<List<PrimaryFeatureValueDto>?> GetPrimaryCatalogItemFeaturesValue(int? itemId)
        {
            var url = $"{_configuration["Endpoints:CatalogAPI"]}/api/primary-catalog/catalog-features-values?id={itemId}&api-version=1";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();
            CatalogItemFeatures = JsonSerializer.Deserialize<List<PrimaryFeatureValueDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return CatalogItemFeatures;

        }
    }



    public class CatalogItemResult
    {
        

    }
}
