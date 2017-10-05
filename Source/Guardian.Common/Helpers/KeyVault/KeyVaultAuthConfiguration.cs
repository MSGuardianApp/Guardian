
namespace Guardian.Common.Helpers
{
    /// <summary>
    /// Configuration for fetching AAD token from certificate
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class KeyVaultAuthConfiguration
    {
        /// <summary>
        /// Client ID of the AAD App
        /// </summary>
        public string ClientId { get; set; }
        
        /// <summary>
        /// Certificate thumbprint
        /// </summary>
        public string CertificateThumbprint { get; set; }

        /// <summary>
        /// Certificate store location
        /// </summary>
        public string CertificateStoreLocation { get; set; }
    }
}