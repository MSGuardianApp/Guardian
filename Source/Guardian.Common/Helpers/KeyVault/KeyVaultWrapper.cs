namespace Guardian.Common.Helpers
{
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Wrapper class to perform lookup operations for secret values against the Key Vault service.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class KeyVaultWrapper : IKeyVaultWrapper
    {
        private KeyVaultClient _keyVaultClient;
        private readonly ICertificateProvider _certificateProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyVaultWrapper"/> class.
        /// </summary>
        /// <param name="certificateProvider">The certificate provider.</param>
        public KeyVaultWrapper(ICertificateProvider certificateProvider)
        {
            _certificateProvider = certificateProvider;
        }
        /// <summary>
        /// Initializes key vault to use the authentication callback provided by the client. 
        /// </summary>
        /// <param name="authenticationCallback"> Key Vault authentication callback.</param>
        public void Initialize(KeyVaultClient.AuthenticationCallback authenticationCallback)
        {
            if (_keyVaultClient == null)
            {
                _keyVaultClient = new KeyVaultClient(authenticationCallback);
            }
        }

        /// <summary>
        /// Initializes key vault by creating authentication callback internally using the client authentication identity and certificate.
        /// </summary>
        /// <param name="keyVaultAuthClientId">The AAD client identifier for the current application.</param>
        /// <param name="keyVaultAuthCertThumbprint">Thumbprint for the certificate used by the application as credential to authenticate with AAD.</param>
        /// <param name="keyVaultAuthCertStoreLocation">The certificate store location.</param>
        public void Initialize(string keyVaultAuthClientId, string keyVaultAuthCertThumbprint, string keyVaultAuthCertStoreLocation)
        {
            if (_keyVaultClient == null)
            {
                var certificate = _certificateProvider.LoadCertificate(keyVaultAuthCertThumbprint, keyVaultAuthCertStoreLocation);
                var assertionCert = new ClientAssertionCertificate(keyVaultAuthClientId, certificate);

                // Initializes key vault with a local authentication callback.
                Initialize(new KeyVaultClient.AuthenticationCallback((authority, resource, scope) => GetAccessToken(authority, resource, scope, assertionCert)));
            }
        }

        /// <summary>
        /// Gets a secret value from key vault, provided a valid secret identifier.
        /// </summary>
        /// <param name="secretIdentifier">The URL for the secret.</param>
        /// <returns>
        /// A response message containing the secret.
        /// </returns>
        public async Task<SecretBundle> GetSecretAsync(string secretIdentifier)
        {
            if (_keyVaultClient == null)
            {
                throw new Exception("Initialize needs to be called before any attempts to retrieve secrets from the key vault.");
            }
            var secret = await _keyVaultClient.GetSecretAsync(secretIdentifier).ConfigureAwait(false);
            return secret;
        }

        /// <summary>
        /// Callback method used to get the access token for key vault, using certificate credentials.
        /// </summary>
        /// <param name="authority">Address of the authority to issue the token.</param>
        /// <param name="resource">Identifier of the requested resource which is recepient of the issued token.</param>
        /// <param name="scope">The scope of the authentication request.</param>
        /// <param name="assertionCert">The client assertion certificate.</param>
        /// <returns>The access token.</returns>
        private async Task<string> GetAccessToken(string authority, string resource, string scope, ClientAssertionCertificate assertionCert)
        {
            var context = new AuthenticationContext(authority, TokenCache.DefaultShared);
            var result = await context.AcquireTokenAsync(resource, assertionCert);
            return result.AccessToken;
        }
    }
}