using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace eShop.ServiceDefaults;

public static class KeyVaultExtensions
{
    /// <summary>
    /// Adds Azure Key Vault integration to the configuration pipeline.
    /// </summary>
    /// <param name="builder">The host application builder.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddAzureKeyVault(this IHostApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var keyVaultUri = configuration["KeyVault:Endpoint"];
        if (string.IsNullOrEmpty(keyVaultUri))
        {
            throw new InvalidOperationException("KeyVault endpoint is not configured.");
        }

        var secretClient = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        builder.Services.AddSingleton(secretClient);


        return builder.Services;
    }
}
