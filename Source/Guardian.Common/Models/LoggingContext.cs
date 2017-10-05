namespace Guardian.Common.Models
{
    /// <summary>
    /// Class that capture the logging context of the incomming request
    /// </summary>
    public class LoggingContext
    {
        /// <summary>
        /// Property set to find the logging context reasons across layers
        /// </summary>
        public System.Guid CorrelationId { get; set; }
        /// <summary>
        /// Correlation vector for a given request
        /// </summary>
        public string CorrelationVector { get; set; }
    }
}
