// <copyright file="ICertificateProvider.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Common.Helpers
{
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// ICertificateProvider
    /// </summary>
    public interface ICertificateProvider
    {
        /// <summary>
        /// Utility method to retrieve the certificate based on the provided thumbprint and store location.
        /// </summary>
        /// <param name="certificateThumbprint">The certificate thumbprint.</param>
        /// <param name="certificateStoreLocation">The certificate store location.</param>
        /// <returns>The certificate object.</returns>
        X509Certificate2 LoadCertificate(string certificateThumbprint, string certificateStoreLocation);
    }
}