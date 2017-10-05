// <copyright file="CertificateProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Common.Helpers
{
    using System;
    using System.Globalization;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// CertificateProvider
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public class CertificateProvider : ICertificateProvider
    {
        /// <summary>
        /// Utility method to retrieve the certificate based on the provided thumbprint and store location.
        /// </summary>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="certificateStoreLocation">The certificate store location.</param>
        /// <returns>The certificate object.</returns>
        public X509Certificate2 LoadCertificate(string certificateThumbprint, string certificateStoreLocation)
        {
            if (string.IsNullOrWhiteSpace(certificateThumbprint))
            {
                throw new System.ArgumentNullException(nameof(certificateThumbprint));
            }

            if (string.IsNullOrWhiteSpace(certificateStoreLocation))
            {
                throw new System.ArgumentNullException(nameof(certificateStoreLocation));
            }

            StoreLocation storeLocation;
            if (!Enum.TryParse<StoreLocation>(certificateStoreLocation, true, out storeLocation))
            {
                throw new System.ArgumentException("Invalid certificate store specified, expected values: LocalMachine or CurrentUser.", nameof(certificateStoreLocation));
            }

            X509Store certificateStore = new X509Store(StoreName.My, storeLocation);
            try
            {
                certificateStore.Open(OpenFlags.ReadOnly);
                X509Certificate2Collection x509Certificate2Collection =
                    certificateStore.Certificates.Find(X509FindType.FindByThumbprint, certificateThumbprint, true);
                if (x509Certificate2Collection == null ||
                    x509Certificate2Collection.Count == 0)
                {
                    throw new Exception(
                        string.Format(CultureInfo.InvariantCulture,
                            "Unable to load the certificate with thumbprint {0} from the store location: {1}.",
                            certificateThumbprint, certificateStoreLocation));
                }
                return x509Certificate2Collection[0];
            }
            finally
            {
                certificateStore.Close();
            }
        }
    }
}