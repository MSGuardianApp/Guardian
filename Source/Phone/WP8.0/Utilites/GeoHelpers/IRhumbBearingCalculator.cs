﻿namespace SOS.Phone.Pages
{
    /// <summary>
    /// This interface can be implemented by classes that
    /// can calculate the rhumb bearing between two positions.
    /// </summary>
    
    public interface IRhumbBearingCalculator
    {
        /// <summary>
        /// Calculate the rhumb bearing between two positions.
        /// </summary>
        double CalculateRhumbBearing(Position position1, Position position2);
    }
}
