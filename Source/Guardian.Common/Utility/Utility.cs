// <copyright file="Utility.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Common
{
    using System;
    /// <summary>
    /// Contains utility methods used across the application
    /// </summary>
    public static class Utility
    {
        /// <summary>
        /// Default Guid extension
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        public static bool IsDefaultGuid(this Guid guid)
        {
            return Guid.Equals(guid, Guid.Empty);
        }
    }
}
