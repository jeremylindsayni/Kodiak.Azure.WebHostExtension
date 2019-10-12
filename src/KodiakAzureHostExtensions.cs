using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Hosting;
using System;

namespace Kodiak.Azure.HostExtension
{
    public static class KodiakAzureHostExtensions
    {
        /// <summary>
        ///     Adds the azure key vault secrets to configuration.
        /// </summary>
        /// <param name="hostBuilder">The host builder.</param>
        /// <param name="keyVaultEndpoint">The key vault endpoint.</param>
        /// <returns></returns>
        /// <exception cref="UriFormatException">This is not a valid secure vault uri</exception>
        public static IHostBuilder AddAzureKeyVaultSecretsToConfiguration(this IHostBuilder hostBuilder, string keyVaultEndpoint)
        {
            if (!IsUriValid(keyVaultEndpoint)) throw new UriFormatException("This is not a valid secure vault uri.");
            
            hostBuilder.ConfigureAppConfiguration((ctx, builder) =>
            {
                var azureServiceTokenProvider = new AzureServiceTokenProvider();
                var keyVaultClient =
                    new KeyVaultClient(
                        new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
                builder.AddAzureKeyVault(keyVaultEndpoint, keyVaultClient, new DefaultKeyVaultSecretManager());
            });

            return hostBuilder;
        }

        /// <summary>
        ///     Determines whether the specified URI is valid].
        /// </summary>
        /// <param name="uri">The URI.</param>
        /// <returns>
        ///     <c>true</c> if the specified URI is valid; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="UriFormatException">
        /// </exception>
        private static bool IsUriValid(string uri)
        {
            var isValidUri = Uri.TryCreate(uri, UriKind.Absolute, out var validatedUri);

            if (!isValidUri) throw new UriFormatException($"'{uri}' is not a valid uri.");

            if (validatedUri.Scheme != Uri.UriSchemeHttps)
                throw new UriFormatException($"'{uri}' does not have Transport Layer Security (i.e. it is not https).");

            return true;
        }
    }
}