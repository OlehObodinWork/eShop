using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Catalog.API.Model
{
    public class PrimaryCatalogItemVariant
    {
        public int Id { get; set; }

        public int ProductId { get; set; }


        [JsonPropertyName("pid")]
        public string ProudctIdString { get; set; }


        [JsonPropertyName("vid")]
        public string VariantId { get; set; }


        [JsonPropertyName("variantImage")]
        [MaxLength(200)]
        public string VariantImageOrigin { get; set; }

        [MaxLength(200)]
        public string VarianImageEnhanced { get; set; }


        [JsonPropertyName("variantSku")]
        public string VariantSKU { get; set; }

        [JsonPropertyName("variantKey")]
        public string VariantKey { get; set; }

        public string VarianKeyAdjust { get; set; }


        [JsonPropertyName("variantLength")]
        public decimal VariantLength { get; set; }

        [JsonPropertyName("variantHeight")]
        public decimal VariantHeigt { get; set; }

        [JsonPropertyName("variantWidth")]
        public decimal VariantWith { get; set; }

        [JsonPropertyName("variantVolume")]
        public decimal VariatVolume { get; set; }

        [JsonPropertyName("variantSellPrice")]
        public decimal VariantPrice { get; set; }

        [JsonPropertyName("variantSugSellPrice")]
        public decimal VariantSellPrice { get; set; }


        [JsonPropertyName("variantStandard")]
        public string VariantStandart { get; set; }


        public decimal VariantFinallPrice { get; set; }

        public int PrimaryCatalogItemId { get; set; } // Required foreign key property

        [JsonIgnore]
        public PrimaryCatalogItem PrimaryCatalogItem { get; set; } = null!;



        public void VarianPriceAdjustments()
        {
            var maxMarginPercent = 333.333m;

            var currentMarginPercent = VariantSellPrice / VariantPrice * 100;

            if (currentMarginPercent > maxMarginPercent)
            {
                VariantFinallPrice = Math.Round(VariantPrice * maxMarginPercent / 100, 2);
            } else
            {
                VariantFinallPrice = VariantSellPrice;
            }
        }

        public void VarianKeyAdjusted()
        {
            var keys = VariantKey.Split('-');

            int shoeSize;

            var isShoeSize = int.TryParse(keys[1], out shoeSize);

            if (isShoeSize)
            {
                shoeSize -= 2;

                VarianKeyAdjust = $"{keys[0]}-{shoeSize}";
                
            } else
            {
                string adjustedSize = keys[1] switch
                {
                    "6XL" => "4XL",
                    "5XL" => "3XL",
                    "4XL" => "2XL",
                    "3XL" => "XL",
                    "2XL" => "L",
                    "XL" => "M",
                    "L" => "S",
                    "M" => "XS",
                    "S" => "XS", // Adjusting S to XS since two sizes lower from S would be beyond XS.
                    _ => keys[1] // Keeping XS as XS, since there is no smaller size.
                };
                VarianKeyAdjust = $"{keys[0]}-{adjustedSize}";
            }
        }

    }
}
