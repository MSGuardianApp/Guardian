using System;

namespace SOS.Service.Security
{
    public class AuthTokenValidationResult
    {
        public bool IsValid { get; protected internal set; }

        public bool IsExpired { get; protected internal set; }

        public DateTime Expiration { get; protected internal set; }

        public bool SourceCheckPassed { get; protected internal set; }

        public string UserID { get; protected internal set; }

    }

}
