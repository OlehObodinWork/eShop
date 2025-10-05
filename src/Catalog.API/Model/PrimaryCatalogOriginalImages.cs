

using System.Reflection.Metadata;

namespace Catalog.API.Model
{
    public class PrimaryCatalogOriginalImages
    {
        public int Id { get; set; }
        public string Src { get; set; }

        public int PrimaryCatalogItemId { get; set; } // Required foreign key property
        public PrimaryCatalogItem PrimaryCatalogItem { get; set; } = null!;
    }
}
