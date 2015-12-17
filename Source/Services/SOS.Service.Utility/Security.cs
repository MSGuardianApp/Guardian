using System;
using System.Text;
using System.Security.Cryptography;

namespace SOS.Service.Utility
{
    public static class Security
    {
        static string key = "Nr9no3frd3ncUeFYv9IhN316CEP/mkcQ";
        public static string Encrypt(string data)
        {
            try
            {
                TripleDESCryptoServiceProvider encryptor = new TripleDESCryptoServiceProvider();

                byte[] inputArray = UTF32Encoding.UTF32.GetBytes(data);

                encryptor.Key = Convert.FromBase64String(key);
                encryptor.Mode = CipherMode.ECB;
                encryptor.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = encryptor.CreateEncryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                encryptor.Clear();

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch { return data; }
        }

        public static string Decrypt(string encryptedData)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(encryptedData);
                TripleDESCryptoServiceProvider tripleDES = new TripleDESCryptoServiceProvider();
                tripleDES.Key = Convert.FromBase64String(key);
                tripleDES.Mode = CipherMode.ECB;
                tripleDES.Padding = PaddingMode.PKCS7;
                ICryptoTransform cTransform = tripleDES.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                tripleDES.Clear();
                return UTF32Encoding.UTF32.GetString(resultArray);
            }
            catch { return encryptedData; }
        }

        public static bool AuthenticateRequest(string authID, string liveID, string RawUri)
        {
            string[] UriParts  = RawUri.Split('/');

            //if (UriParts.Count() >= 3)
            //{
            //    if (UriParts[2] == "GetProfilesForLiveID")
            //        return false;
            //    else
            //        return true;
            //}
            return true;
        }
    }
}
