namespace SOS.Phone.Pages
{
    /// <summary>
    /// This interface can be implemented by all classes
    /// that can convert angles in various ways.
    /// </summary>
    
    public interface IAngleConverter
    {
        /// <summary>
        /// Convert degrees to radians.
        /// </summary>
        double ConvertDegreesToRadians(double degrees);

        /// <summary>
        /// Convert radians to degrees.
        /// </summary>
        double ConvertRadiansToDegrees(double radians);
    }
}