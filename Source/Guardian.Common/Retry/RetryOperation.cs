// <copyright file="RetryOperation.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace Guardian.Common.Retry
{
    using System;
    using System.Threading.Tasks;
    using Guardian.Common.Exceptions;
    /// <summary>
    /// Retries a given operation by given number of turns
    /// </summary>
    public static class RetryOperation
    {
        /// <summary>
        /// Retries the operation without a return value
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public static async Task RetryableOperationAsync(Func<Task> operation, int retryCount = 3)
        {
            try
            {
                await operation.Invoke();
            }
            catch (Exception ex)
            {
                if (retryCount == 1 || ex.IsFatalException() )
                {
                    throw;
                }
                await RetryableOperationAsync(operation, retryCount - 1);
            }
        }

        /// <summary>
        /// Retries the operation with a return value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="operation"></param>
        /// <param name="retryCount"></param>
        /// <returns></returns>
        public static async Task<T> RetryableOperationWithReturnAsync<T>(Func<Task<T>> operation, int retryCount = 3)
        {
            try
            {
                return await operation.Invoke();
            }
            catch (Exception ex)
            {
                if (retryCount == 1 || ex.IsFatalException())
                {
                    throw;
                }
                return await RetryableOperationWithReturnAsync(operation, retryCount - 1);
            }
        }
    }
}
