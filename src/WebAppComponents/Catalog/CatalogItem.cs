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


public class CatalogFeature
{

    public string? Icon { get; set; }

    public string? TitleEN { get; set; }

    public string? TitleDE { get; set; }

}



public record CatalogResult(int PageIndex, int PageSize, int Count, List<CatalogItem> Data);
public record CatalogBrand(int Id, string Brand);
public record CatalogItemType(int Id, string Type);
