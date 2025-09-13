using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using CJDropship.API.Services.DTO;
using CJDropship.API.Services.Interfaces;

namespace CJDropship.API.Services
{

    public class CJTokenService : ITokenService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<CJTokenService> _logger;
        public string? token { get; set; }  
        public string? refreshToken;
        private readonly IConfiguration _configuration;
        public DateTime tokenExpiration;

        public CJTokenService(IHttpClientFactory httpClientFactory, ILogger<CJTokenService> logger, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<string?> GetTokenAsync()
        {
            if (token == null || DateTime.UtcNow >= tokenExpiration)
            {
                token = await FetchNewTokenAsync();
            }
            return token;
        }

        private async Task<string?> FetchNewTokenAsync()
        {
            // Logic to fetch a new token from the third-party API
            var client = _httpClientFactory.CreateClient();
            var data = "";

            // Rewrite if we wanna make admin work for different accounts
            var keyVaultEndpoint = _configuration["KeyVault:Endpoint"];
            if (string.IsNullOrEmpty(keyVaultEndpoint))
            {
                throw new InvalidOperationException("KeyVault endpoint is not configured.");
            }
            var keyVaultUrl = new Uri(keyVaultEndpoint);
            var secretClient = new SecretClient(keyVaultUrl, new DefaultAzureCredential());

            var email = await secretClient.GetSecretAsync("CJEmail");
            var key = await secretClient.GetSecretAsync("CJKey");

            var credentials = new { email = email.Value.Value, password = key.Value.Value };

            var jsonCredentials = JsonSerializer.Serialize(credentials);
            var url = "https://developers.cjdropshipping.com/api2.0/v1/authentication/getAccessToken";
            var response = await client.PostAsync(url, new StringContent(jsonCredentials, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();


            var jsonResponse = await response.Content.ReadAsStringAsync();

            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;

                // Extract the "data" field
                if (root.TryGetProperty("data", out JsonElement dataElement))
                {
                    data = dataElement.GetRawText();
                    Console.WriteLine($"Data field: {data}");
                }
                else
                {
                    Console.WriteLine("Data field not found.");
                }
            }
            var tokenJson = JsonSerializer.Deserialize<TokenResponseDTO>(data);
 
            if (tokenJson != null)
            {
                //tokenExpiration = DateTime.UtcNow.AddSeconds(tokenJson.ExpiresIn);
                refreshToken = tokenJson.RefreshToken;
                return tokenJson.AccessToken;
            }

            return null;
        }
    }
}
