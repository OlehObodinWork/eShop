using Catalog.API.Model;
using eShop.Catalog.API.Services;

namespace eShop.Catalog.API.Infrastructure;

/// <remarks>
/// Add migrations using the following command inside the 'Catalog.API' project directory:
///
/// dotnet ef migrations add --context CatalogContext [migration-name]
/// </remarks>
public class CatalogContext : DbContext
{
    public CatalogContext(DbContextOptions<CatalogContext> options, IConfiguration configuration) : base(options)
    {
    }

    public DbSet<CatalogItem> CatalogItems { get; set; }
    public DbSet<CatalogBrand> CatalogBrands { get; set; }
    public DbSet<CatalogType> CatalogTypes { get; set; }

    public DbSet<PrimaryCatalogItem> PrimaryCatalogItems { get; set; }
    public DbSet<PrimaryCatalogItemVariant> PrimaryCatalogItemVariants { get; set; }

    public DbSet<PrimaryCatalogEnhancedImages> PrimaryCatalogEnhancedImages { get; set; }

    public DbSet<PrimaryCatalogOriginalImages> PrimaryCatalogOriginalImages { get; set; }

    public DbSet<PrimaryCatalogFeature> PrimaryCatalogFeatures { get; set; }

    public DbSet<PrimaryCatalogKit> PrimaryCatalogKits { get; set; }

    public DbSet<PrimaryCatalogFeatureValue> PrimaryCatalogFeatureValues { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.HasPostgresExtension("vector");
        builder.ApplyConfiguration(new CatalogBrandEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogTypeEntityTypeConfiguration());
        builder.ApplyConfiguration(new CatalogItemEntityTypeConfiguration());

        // Add the outbox table to this context
        builder.UseIntegrationEventLogs();
    }
}
