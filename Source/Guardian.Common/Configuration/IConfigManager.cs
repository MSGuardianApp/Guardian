
namespace Guardian.Common.Configuration
{
    /// <summary>
    /// Interface for configuration settings manager which provides configuration settings for the application.
    /// </summary>
    public interface IConfigManager
    {
        /// <summary>
        /// Settings object for strongly types setting fields.
        /// </summary>
        Settings Settings { get; }
    }
}