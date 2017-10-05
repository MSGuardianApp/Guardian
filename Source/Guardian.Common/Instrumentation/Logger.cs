namespace Guardian.Common.Instrumentation
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Common Logger for App Insights
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Logger instnace
        /// </summary>
        public static ILogger commonLogger;

        /// <summary>
        /// Initializes the common logger
        /// </summary>
        /// <param name="logger"></param>
        public static void Initialize(ILogger logger)
        {
            commonLogger = logger;
        }

        /// <summary>
        /// Gets the instance
        /// </summary>
        public static ILogger Instance
        {
            get { return commonLogger; }
        }

        /// <summary>
        /// Logs custom event
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="message"></param>
        /// <param name="properties"></param>
        public static void TrackEvent(this ILogger logger, string eventName, IDictionary<string, string> properties)
        {
            try
            {
                logger.TrackEvent(eventName, properties);
            }
            catch
            {
                //Swallow any error occurred while loggin
            }
        }

        /// <summary>
        /// Logs exception
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="msg">Base error message.</param>
        /// <param name="ex">Exception data.</param>
        /// <param name="properties">The properties.</param>
        public static void TrackException(ILogger logger, Exception ex, IDictionary<string, string> properties)
        {
            try
            {
                logger.TrackException(ex, properties);
            }
            catch (Exception)
            {
                //Swallow any error occurred while logging
            }
        }
        /// <summary>
        /// Copies the dictionary to a new dictionary
        /// </summary>
        /// <param name="properties">Dictionary object</param>
        /// <returns>Dictionary object</returns>
        public static IDictionary<string, string> Clone(this IDictionary<string, string> properties)
        {
            return new Dictionary<string, string>(properties);
        }

        /// <summary>
        /// Adds property to the dictionary
        /// </summary>
        /// <param name="properties">Dictionary object</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <returns>Dictionary object</returns>
        public static IDictionary<string, string> AddProperty(this IDictionary<string, string> properties, string key, string value)
        {
            properties[key] = value;
            return properties;
        }
    }
}
