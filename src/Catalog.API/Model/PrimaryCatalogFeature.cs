

using System.Text.Json.Serialization;

namespace eShop.Catalog.API.Model
{
    public class PrimaryCatalogFeature
    {
        public int Id { get; set; }
        public string Icon { get; set; }

        public string Title { get; set; }

    }

    public class PrimaryCatalogFeatureValue
    {
        public int Id { get; set; }
        public int PrimaryCatalogItemId { get; set; }

        [JsonIgnore]
        public PrimaryCatalogItem PrimaryCatalogItem { get; set; }

        public int PrimaryCatalogFeatureId { get; set; }

        [JsonIgnore]
        public PrimaryCatalogFeature PrimaryCatalogFeature { get; set; }

        public string Value {get; set;}
    }
}
