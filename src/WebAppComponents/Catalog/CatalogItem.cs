using System.ComponentModel.DataAnnotations;
using System.Numerics;
using System.Text.Json.Serialization;

namespace eShop.WebAppComponents.Catalog;

public record CatalogItem(
    int Id,
    string Name,
    string Description,
    decimal Price,
    string PictureUrl,
    int CatalogBrandId,
    CatalogBrand CatalogBrand,
    int CatalogTypeId,
    CatalogItemType CatalogType);


public class CatalogItemTest
{
    public int Id { get; set; }
}

public class CatalogItemDetailDto
{
    public string? NameEN { get; set; }
    public string? NameDE { get; set; }
    public string? Description { get; set; }
    public string? ProductNameEn { get; set; }
    public string? DescriptionEN { get; set; }
    public string? DescriptionDE { get; set; }
    public string? PackingNameEN { get; set; }
    public string? PackingNameDE { get; set; }
    public string? ProductKeyEn { get; set; }
    public string? ProductKenDE { get; set; }
    public string? CategoryNameEN { get; set; }
    public string? CategoryNameDE { get; set; }

    public List<CatalogItemVariantDto>? CatalogItemVariants { get; set; }
}


public class CatalogItemVariantDto
{

    public string? VariantImage { get; set; }

    public string? VariantKey { get; set; }

    public string? VarianKeyEnAdjusted { get; set; }

    public decimal? VariantFinallPrice { get; set; }
    public string? VariantKeyDE { get; set; }
    public decimal? VariantSellPrice { get; set; }

    public string? VariantKeyNameEN { get; set; }

    public string? VariantKeyNameDE { get; set; }

    public string? VariantKeyValue { get; set; }


}

public record CatalogItemRecord(int Id, string Name, [property: JsonPropertyName("productNameEn")] string OriginName, string NameEN, string NameDE, [property: JsonPropertyName("description")] string Description, string DescriptionDE, string DescriptionEN, [property: JsonPropertyName("productWeight")] string ProductWeight, [property: JsonPropertyName("productType")] string ProducctType, [property: JsonPropertyName("categoryId")] string CategoryId, [property: JsonPropertyName("categoryName")] string CategoryNameEN, string CategoryNameDE, [property: JsonPropertyName("productSku")] string ProductSKU, [property: JsonPropertyName("productKeyEn")] string ProductKeyEN, string ProductKenDE, decimal Price, [property: JsonPropertyName("sellPrice")] string OriginPrice, [property: JsonPropertyName("suggestSellPrice")] string SuggestSellPrice, int ListedNum, string PictureFileName, ICollection<OriginalImages> OriginalImages, ICollection<EnchancedImages> EnhancedImages, List<CatalogFeature> CatalogFeatures, ICollection<CatalogItemVariant> CatalogItemVariants, [property: JsonPropertyName("packingWeight")] string PackingWeight, [property: JsonPropertyName("packingNameEn")] string PackingNameEN, string PackingNameDE, string PackingNameSetEN, string PackingNameSetDE, int CatalogTypeId, int CatalogBrandId, int AvailableStock, int RestockThreshold, int MaxStockThreshold, bool OnReorder);

public record OriginalImages(int Id, string Src, int CatalogItemId, CatalogItem CatalogItem);

public record EnchancedImages(int Id, string Src, int CatalogItemId, CatalogItem CatalogItem);

public record CatalogFeature(int Id, string Icon, string TitleEN, string TitleDE, string ValueEN, string ValueDE, List<CatalogItem> CatalogItems);


public record CatalogItemVariant(int Id, int ProductId, [property: JsonPropertyName("pid")] string ProudctIdString, [property: JsonPropertyName("vid")] string VariantId, [property: JsonPropertyName("variantImage"), MaxLength(200)] string VariantImageOrigin, [property: MaxLength(200)] string VarianImageEnhanced, [property: JsonPropertyName("variantSku")] string VariantSKU, [property: JsonPropertyName("variantKey")] string VariantKeyEN, string VariantKeyDE, [property: JsonPropertyName("variantLength")] decimal VariantLength, [property: JsonPropertyName("variantHeight")] decimal VariantHeigt, [property: JsonPropertyName("variantWidth")] decimal VariantWith, [property: JsonPropertyName("variantVolume")] decimal VariatVolume, [property: JsonPropertyName("variantSellPrice")] decimal VariantPrice, [property: JsonPropertyName("variantSugSellPrice")] decimal VariantSellPrice, [property: JsonPropertyName("variantStandard")] string VariantStandart, int CatalogItemId, CatalogItem CatalogItem);

public record CatalogKit(int Id, string Name, string NameDE, List<CatalogItem> CatalogItems);

public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogItem> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogItemType(int Id, string Type);
