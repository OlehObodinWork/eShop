

namespace eShop.Catalog.API.Model
{
    public class CatalogFeature
    {
        public int Id { get; set; }
        public string Icon { get; set; }

        public string TitleEN { get; set; }

        public string TitleDE { get; set; }


        public List<CatalogItem> CatalogItems { get; set; }
    }

    public class CatalogFeatureValue
    {
        public int Id { get; set; }
        public int CatalogItemId { get; set; }
        public CatalogItem CatalogItem { get; set; }

        public int CatalogFeatureId { get; set; }
        public CatalogFeature CatalogFeature { get; set; }

        public string ValueEN {get; set;}
        public string ValueDE { get; set; }
    }
}
