// <copyright file="ExceptionExtensions.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Common.Exceptions
{
    using System;
    using System.Threading;
    /// <summary>
    /// Provides useful exception extension methods.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Checks if an exception is fatal.
        /// </summary>
        /// <param name="ex">The exception to check.</param>
        /// <returns>True if Exception is fatal.</returns>
        public static bool IsFatalException(this Exception ex)
        {
            return ex != null && (ex is OutOfMemoryException || ex is AppDomainUnloadedException || ex is BadImageFormatException
                || ex is CannotUnloadAppDomainException || ex is InvalidProgramException || ex is ThreadAbortException || ex is StackOverflowException);
        }
    }
}
