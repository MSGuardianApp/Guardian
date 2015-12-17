using System;

namespace SOS.Phone
{
    public static partial class Extensions
    {
        public static string GetValue(this string inputString)
        {
            return String.IsNullOrEmpty(inputString) ? String.Empty : inputString.Trim();
        }
    }

}
