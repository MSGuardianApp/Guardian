using System;
using System.Runtime.Serialization;
using SOS.Service.Utility;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Selectors;
using System.Security.Claims;
using System.Linq;


namespace SOS.Service.Security
{
    public sealed class AuthTokenValidator : IDisposable
    {

        private string _AuthToken = string.Empty;

        private string _CurrentSecretKey = string.Empty;

        private JWTEnvelope _Envelope = null;
        private JWTClaims _Claims = null;

        private RawLiveAuthToken _RawToken = null;

        private bool _IsTokenValid = false;

        public AuthTokenValidationResult Result { get; private set; }

        public AuthTokenValidator(string AuthToken, AuthTokenType authTokenType = AuthTokenType.LiveAuthToken)
        {
            ValidateAndSplitToken(AuthToken);
            ProcessToken(authTokenType, AuthToken);
        }

        private void ProcessToken(AuthTokenType authTokenType = AuthTokenType.LiveAuthToken, string IdToken = "")
        {
            ExtractEnvelopeInfo();
            //Validate the envelope Info

            ExtractClaimsInfo();

            if (authTokenType == AuthTokenType.LiveAuthToken)
            {
                VerifySignatureInfo();
                LoadBaseResults();
            }
            else if(authTokenType == AuthTokenType.GoogleToken)
            {
                VerifyGoogleToken(IdToken);
                VerifyAndLoadGoogleBaseResults();
            }
        }

        private void VerifyAndLoadGoogleBaseResults()
        {
            if (_Claims == null)
                throw new Exception("Claims Not Loaded. ");
            
            Result = new AuthTokenValidationResult();

            Result.IsExpired = _Claims.Expiration < DateTime.Now;
            Result.Expiration = _Claims.Expiration;
            Result.IsValid = _IsTokenValid;

            Result.SourceCheckPassed = GoogleClientIDCrossCheck();

            if (!Result.SourceCheckPassed)
            {
                Result.IsValid = false;
                Result.IsExpired = true;
            }
            else
            {
                Result.UserID = _Claims.sub;
                Result.IsValid = true;
            }
        }

        public void VerifyGoogleToken(string idToken)
        {
            if (idToken != null)
            {
                try
                {
                    JwtSecurityToken token = new JwtSecurityToken(idToken);
                    JwtSecurityTokenHandler jsth = new JwtSecurityTokenHandler();
                    string audience = token.Audiences.ToString();

                    Byte[][] certBytes = SOSCodecs.getGoogleCertBytes();
                    Dictionary<String, X509Certificate2> certificates = new Dictionary<string, X509Certificate2>();

                    for (int i = 0; i < certBytes.Length; i++)
                    {
                        X509Certificate2 certificate = new X509Certificate2(certBytes[i]);
                        certificates.Add(certificate.Thumbprint, certificate);
                    }
                    // Set up token validation

                    TokenValidationParameters tvp = new TokenValidationParameters()
                    {
                        ValidateActor = false,
                        ValidAudience = ConfigProvider.ConfigurationStore.GoogleClientID,
                        ValidateIssuer = true,
                        ValidIssuer = "accounts.google.com",
                        ValidateIssuerSigningKey = true,
                        RequireSignedTokens = true,
                        CertificateValidator = X509CertificateValidator.None,
                        IssuerSigningKeyResolver = (s, securityToken, identifier, parameter) =>
                        {
                            return identifier.Select(x =>
                            {
                                if (certificates.ContainsKey(x.Id.ToUpper()))
                                {
                                    return new X509SecurityKey(certificates[x.Id.ToUpper()]);
                                }
                                return null;
                            }).First(x => x != null);
                        },
                        ValidateLifetime = false,
                        RequireExpirationTime = true,
                        ClockSkew = TimeSpan.FromHours(12)
                    };


                    SecurityToken validateToken;
                    ClaimsPrincipal cp = jsth.ValidateToken(idToken, tvp, out validateToken);
                    if (cp != null)
                    {
                        _IsTokenValid = true;
                    }

                }
                catch (Exception e)
                {
                    _IsTokenValid = false;
                }
            }
        }

        private bool GoogleClientIDCrossCheck()
        {
            if (ConfigProvider.ConfigurationStore.GoogleClientID == _Claims.aud && _Claims.iss.Contains("accounts.google.com"))
                return true;
            else
                return false;
        }

        private void LoadBaseResults()
        {
            if (_Claims == null)
                throw new Exception("Claims Not Loaded. ");

            Result = new AuthTokenValidationResult();

            Result.IsExpired = _Claims.Expiration < DateTime.Now;
            Result.Expiration = _Claims.Expiration;
            Result.IsValid = _IsTokenValid;

            Result.SourceCheckPassed = SourcesCrossCheck();


            if (!Result.SourceCheckPassed)
            {
                Result.IsValid = false;
                Result.IsExpired = true;
            }
            else
                Result.UserID = _Claims.uid;
        }        

        private bool SourcesCrossCheck()
        {          
            return ConfigProvider.ConfigurationStore.LiveAuthAppURI == _Claims.aud;
        }

        private void ExtractClaimsInfo()
        {
            string claimsText = _RawToken.Claims;
            try
            {
                _Claims = SOSCodecs.Deserialize(claimsText, typeof(JWTClaims)) as JWTClaims;
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("Failed To Deserialize Base 64 encoded JWT Claims to JSON Object. Text:{0}", claimsText),
                        ex);
            }
        }
        
        private void VerifySignatureInfo()
        {
            int ikid = 0;
            if (!int.TryParse(_Envelope.kid, out ikid))
                throw new ArgumentOutOfRangeException("Key ID should be a number more than 0, PassedVal:" + _Envelope.kid);

            if (ikid > ConfigProvider.ConfigurationStore.LiveAuthKeyCount)
                throw new ArgumentOutOfRangeException(string.Format("Key ID: {0}, is not configured properly or not loaded.", ikid));

            _CurrentSecretKey = ConfigProvider.ConfigurationStore.LiveAuthKeys[ikid];

            byte[] bKey = SOSCodecs.UTF8Encoder.GetBytes(_CurrentSecretKey + "JWTSig");

            SHA256Managed SHAprovider = SecurityCodecs.SHA256CryptoProvider;

            byte[] bCryptKey = SHAprovider.ComputeHash(bKey);

            byte[] bCombined = SOSCodecs.UTF8Encoder.GetBytes(_RawToken.Envelope + "." + _RawToken.Claims);

            SecurityCodecs codec = new SecurityCodecs();

            HMACSHA256 HMACHACryptoProvider = codec.HMACSHA256Provider(bCryptKey);
            _IsTokenValid = SOSCodecs.UrlEncode(HMACHACryptoProvider.ComputeHash(bCombined)) == _RawToken.Signature;

            codec.Dispose();

            SHAprovider.Clear();
            SHAprovider.Dispose();

            HMACHACryptoProvider.Clear();
            HMACHACryptoProvider.Dispose();
        }
        
        private void ExtractEnvelopeInfo()
        {
            string envelopeText = _RawToken.Envelope;
            try
            {
                _Envelope = SOSCodecs.Deserialize(envelopeText, typeof(JWTEnvelope)) as JWTEnvelope;
            }
            catch (Exception ex)
            {
                throw new SerializationException(string.Format("Failed To Deserialize Base 64 encoded JWT Envelope to JSON Object. Text:{0}", envelopeText),
                        ex);
            }
        }

        public void ValidateAndSplitToken(string authToken)
        {
            if (string.IsNullOrWhiteSpace(authToken))
                throw new ArgumentNullException("AuthToken Not Provided");

            _AuthToken = authToken;

            if (!_AuthToken.Contains("."))
                throw new Exception("AuthToken is Invalid");


            string[] tokenParts = _AuthToken.Split('.');

            if (tokenParts.Length != 3 || string.IsNullOrWhiteSpace(tokenParts[0]) || string.IsNullOrWhiteSpace(tokenParts[1]) || string.IsNullOrWhiteSpace(tokenParts[2]))
                throw new Exception("AuthToken/Parts of it is Invalid");

            _RawToken = new RawLiveAuthToken(tokenParts[0], tokenParts[1], tokenParts[2]);
        }

        public void Dispose()
        {
            _AuthToken = null;
            _Claims = null;
            _CurrentSecretKey = null;
            _Envelope = null;
            _RawToken = null;

        }



        #region Sub Classes

        private class RawLiveAuthToken
        {
            internal string Envelope, Claims, Signature;
            internal RawLiveAuthToken(string env, string clm, string sign)
            { Envelope = env; Claims = clm; Signature = sign; }
        }

        [DataContract]
        private class JWTEnvelope
        {
            [DataMember]
            internal string typ, alg, kid;
        }

        [DataContract]
        private class JWTClaims
        {

            [DataMember]
            private int exp;

            private DateTime? expiration = null;
            public DateTime Expiration
            {
                get
                {
                    if (this.expiration == null)
                    {
                        this.expiration = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(exp);
                    }

                    return (DateTime)this.expiration;
                }
            }

            [DataMember]
            public string iss, aud, uid, sub;

            [DataMember(Name = "urn:microsoft:appuri")]
            public string appuri;

            [DataMember(Name = "urn:microsoft:appid")]
            public string appid;
        }


        public enum AuthTokenType
        {
            LiveAuthToken = 1,
            GoogleToken = 2,
            FaceBookToken = 3
        }

        #endregion Sub Classes
    }

}
