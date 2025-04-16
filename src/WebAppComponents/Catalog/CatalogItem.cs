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

    [JsonPropertyName("productNameEn")]
    public string? OriginName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ProductName { get; set; }
    public string? PackingName { get; set; }
    public string? ProductKey { get; set; }
    public string? CategoryName { get; set; }

    public List<CatalogItemVariantDto>? CatalogItemVariants { get; set; }
}

public class CatalogFeatureDto
{
    public string? Icon { get; set; }
    public string? Title { get; set; }
}


public class CatalogItemVariantDto
{

    public string? VariantImage { get; set; }

    public string? VariantKey { get; set; }

    public string? VarianKeyAdjust { get; set; }

    public decimal? VariantFinallPrice { get; set; }
    public decimal? VariantSellPrice { get; set; }

    public string? VariantKeyName { get; set; }


    public string? VariantKeyValue { get; set; }


}



public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogItem> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogItemType(int Id, string Type);
