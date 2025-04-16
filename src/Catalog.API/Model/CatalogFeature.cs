

using System.Text.Json.Serialization;

namespace eShop.Catalog.API.Model
{
    public class CatalogFeature
    {
        public int Id { get; set; }
        public string Icon { get; set; }

        public string Title { get; set; }

    }

    public class CatalogFeatureValue
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }

        [JsonIgnore]
        public CatalogItem CatalogItem { get; set; }

        public int CatalogFeatureId { get; set; }

        [JsonIgnore]
        public CatalogFeature CatalogFeature { get; set; }

        public string Value {get; set;}
    }
}
