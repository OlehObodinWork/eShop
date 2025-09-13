using System.Text.Json.Serialization;

namespace CJDropship.API.Services.DTO
{
    public class TokenResponseDTO
    {
        [JsonPropertyName("accessToken")]
        public string? AccessToken { get; set; }

        [JsonPropertyName("accessTokenExpiryDate")]
        public string? AccessTokenExpirity { get; set; }
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
    }
}
