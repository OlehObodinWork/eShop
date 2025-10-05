namespace Catalog.API.Model
{
    public class PrimaryCatalogKit
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<PrimaryCatalogItem> PrimaryCatalogItems { get; set; }
    }
}
