namespace Guardian.Common.Exceptions
{
    using Guardian.Common.Instrumentation;
    using Guardian.Common.Models;
    using Guardian.Common.Retry;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Exception handler for the service layer
    /// </summary>
    public static class CustomExceptionHandler
    {
        /// <summary>
        /// Handles potential exceptions that may be thrown by code that performs a web API read network request.
        /// All exceptions will be converted into a <see cref="ServiceException" />.
        /// </summary>
        /// <typeparam name="T">The type the passed in operation returns.</typeparam>
        /// <param name="operation">The function to execute and handle exceptions from if needed.</param>
        /// <param name="loggingContext">The logging context.</param>
        /// <returns>
        /// The function result, or throws an exception according to the configured behavior.
        /// </returns>
        public static async Task HandleOperationAsync(Func<Task> operation, LoggingContext loggingContext, int retryCount = 1)
        {
            try
            {
                // invoke the operation
                await RetryOperation.RetryableOperationAsync(operation);
            }
            catch (Exception exception)
            {
                //AggregateException aggregateException
                Logger.Instance.TrackException(exception);
                throw;
            }
        }

        /// <summary>
        /// Handles potential exceptions that may be thrown by code that performs a web API network request.
        /// </summary>
        /// <param name="operation">A function (that return a value) to execute and handle exceptions from if needed.</param>
        /// <param name="loggingContext">The logging context.</param>
        /// <returns>
        /// The function result, or throws an exception according to the configured behavior.
        /// </returns>
        public static async Task<T> HandlerOperationAsync<T>(Func<Task<T>> operation, LoggingContext loggingContext, int retryCount = 1)
        {
            try
            {
                // invoke the operation
                return await RetryOperation.RetryableOperationWithReturnAsync(operation);
            }
            catch (Exception exception)
            {
                //AggregateException aggregateException
                Logger.Instance.TrackException(exception);
                throw;
            }
        }
    }
}