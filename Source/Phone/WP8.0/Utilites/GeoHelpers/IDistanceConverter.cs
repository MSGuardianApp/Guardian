namespace SOS.Phone.Pages
{
    /// <summary>
    /// This interface can be implemented by all classes
    /// that can convert distances in various ways.
    /// </summary>
    
    public interface IDistanceConverter
    {
        /// <summary>
        /// Convert kilometers to miles.
        /// </summary>
        double ConvertKilometersToMiles(double kilometers);

        /// <summary>
        /// Convert miles to kilometers.
        /// </summary>
        double ConvertMilesToKilometers(double miles);
    }
}