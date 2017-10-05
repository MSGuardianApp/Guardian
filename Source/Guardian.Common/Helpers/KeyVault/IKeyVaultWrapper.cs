
namespace Guardian.Common.Helpers
{
    using Microsoft.Azure.KeyVault;
    using Microsoft.Azure.KeyVault.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Interface to perform lookup operations for secret values against the Key Vault service.
    /// </summary>
    public interface IKeyVaultWrapper
    {
        /// <summary>
        /// Initializes key vault to use the authentication callback provided by the client. 
        /// </summary>
        /// <param name="authenticationCallback"> Key Vault authentication callback.</param>
        void Initialize(KeyVaultClient.AuthenticationCallback authenticationCallback);

        /// <summary>
        /// Initializes key vault by creating authentication callback internally using the client authentication identity and certificate.
        /// </summary>
        /// <param name="keyVaultAuthClientId">The AAD client identifier for the current application.</param>
        /// <param name="keyVaultAuthCertThumbprint">Thumbprint for the certificate used by the application as credential to authenticate with AAD.</param>
        /// <param name="keyVaultAuthCertStoreLocation">The certificate store location.</param>
        void Initialize(string keyVaultAuthClientId, string keyVaultAuthCertThumbprint, string keyVaultAuthCertStoreLocation);

        /// <summary>
        /// Gets a secret value from key vault, provided a valid secret identifier.
        /// </summary>
        /// <param name="secretIdentifier">The URL for the secret.</param>
        /// <returns>
        /// A response message containing the secret.
        /// </returns>
        Task<SecretBundle> GetSecretAsync(string secretIdentifier);
    }
}