﻿namespace SOS.Phone.Pages
{
    /// <summary>
    /// This interface can be implemented by classes that
    /// can calculate the rhumb bearing between two positions.
    /// </summary>
    
    public interface IRhumbDistanceCalculator
    {
        /// <summary>
        /// Calculate the rhumb distance between two positions.
        /// </summary>
        double CalculateRhumbDistance(Position position1, Position position2, DistanceUnit distanceUnit);
    }
}
