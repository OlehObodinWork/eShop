namespace Catalog.API.Model
{
    public class PrimaryCatalogEnhancedImages
    {

        public int Id { get; set; }
        public string Src { get; set; }

        public int PrimaryCatalogItemId { get; set; } // Required foreign key property
        public PrimaryCatalogItem PrimaryCatalogItem { get; set; } = null!;
    }
}
