﻿namespace SOS.Phone.Pages
{
    /// <summary>
    /// This interface can be implemented by classes that
    /// can calculate the bearing between two positions.
    /// </summary>
    
    public interface IBearingCalculator
    {
        /// <summary>
        /// Calculate the bearing between two positions.
        /// </summary>
        double CalculateBearing(Position position1, Position position2);
    }
}
