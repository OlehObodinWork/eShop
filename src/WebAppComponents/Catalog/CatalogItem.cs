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
    public int Id { get; set; }
    [JsonPropertyName("productNameEn")]
    public string? OriginName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ProductName { get; set; }
    public string? PackingName { get; set; }
    public string? ProductKey { get; set; }
    public string? CategoryName { get; set; }

    public List<CatalogItemVariantDto>? PrimaryCatalogItemVariants { get; set; }
}

public class CatalogFeatureDto
{

    public int Id { get; set; }
    public required string Icon { get; set; }
    public required string Title { get; set; }
}

public class CatalogFeatureValueDto
{
    public int PrimaryCatalogFeatureId { get; set; }

    public int PrimaryCatalogItemId { get; set; }

    public string? Value { get; set; }
}


public class CatalogItemVariantDto
{

    public string? variantImage { get; set; }

    public string? variantKey { get; set; }

    public string? varianKeyAdjust { get; set; }
    
    public decimal? variantFinallPrice { get; set; }
    public decimal? variantSellPrice { get; set; }

    public string? variantKeyName { get; set; }


    public string? variantKeyValue { get; set; }


}



public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogItem> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogItemType(int Id, string Type);
