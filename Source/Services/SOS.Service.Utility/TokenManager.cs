using System;

namespace SOS.Service.Utility
{
    public class TokenManager
    {
        public static string GenerateNewValidationID()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateBasePostToken()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateNewTeaserID()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
