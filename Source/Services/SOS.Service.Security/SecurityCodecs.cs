using System;
using System.Security.Cryptography;

namespace SOS.Service.Security
{
    internal sealed class SecurityCodecs:IDisposable
    {
        private string _CryptKeyForRest = string.Empty;

        public SecurityCodecs() {
            LoadKeys();
        }

        private void LoadKeys()
        {
            _CryptKeyForRest = ConfigProvider.ConfigurationStore.CryptKeyForRest;
        }

        private void ClearKeys()
        {
            _CryptKeyForRest = null;
        }
        
        internal static SHA256Managed SHA256CryptoProvider { get { return new SHA256Managed(); } }        

        internal HMACSHA256 HMACSHA256Provider(byte[] CryptKey)
        {
            return new HMACSHA256(CryptKey);
        }

        internal TripleDESCryptoServiceProvider DES3Provider()
        {
            TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider();

            provider.Key = Convert.FromBase64String(_CryptKeyForRest);
            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.PKCS7;

            return provider;
        }

        internal AesCryptoServiceProvider AESProvider()
        {
            AesCryptoServiceProvider provider = new AesCryptoServiceProvider();

            provider.Key = Convert.FromBase64String(_CryptKeyForRest);
            provider.Mode = CipherMode.ECB;
            provider.Padding = PaddingMode.PKCS7;

            return provider;
        }

        public void Dispose()
        {
            ClearKeys();
            //Nullify
        }


    }
}
