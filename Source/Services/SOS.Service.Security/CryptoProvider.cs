using System;
using System.Security.Cryptography;
using System.Text;

namespace SOS.Service.Security
{
    public sealed class RestingDataCryptoProvider
    {
        
        public static string Encrypt(string data)
        {
            try
            {
                byte[] inputArray = UTF32Encoding.UTF32.GetBytes(data);

                SecurityCodecs codec = new SecurityCodecs();
                TripleDESCryptoServiceProvider provider = codec.DES3Provider();
               
                ICryptoTransform cTransform = provider.CreateEncryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);

                provider.Clear();
                provider.Dispose();
                codec.Dispose();

                return Convert.ToBase64String(resultArray, 0, resultArray.Length);
            }
            catch { return data; }
        }

        public static string Decrypt(string encryptedData)
        {
            try
            {
                byte[] inputArray = Convert.FromBase64String(encryptedData);
                
                SecurityCodecs codec = new SecurityCodecs();
                TripleDESCryptoServiceProvider provider = codec.DES3Provider();

                ICryptoTransform cTransform = provider.CreateDecryptor();
                byte[] resultArray = cTransform.TransformFinalBlock(inputArray, 0, inputArray.Length);
                
                provider.Clear();
                provider.Dispose();
                codec.Dispose();

                return UTF32Encoding.UTF32.GetString(resultArray);
            }
            catch { return encryptedData; }
        }

        public static string GenerateRandomDigit()
        {
            //using RNGCryptoServiceProvider - ISRM review.
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buffer = new byte[4];
            rng.GetBytes(buffer);
            int result = BitConverter.ToInt32(buffer, 0);
            int Digits = 5;
            int MinNumber = 10000;
            int MaxNumber = 99999;
            try
            {
                Digits = int.Parse(SOS.ConfigManager.Config.RandomNumberDigits);
                MinNumber = (int)Math.Pow(10, Digits-1);
                MaxNumber = (int)(Math.Pow(10, Digits) - 1);
            }
            catch { }
            return new Random(result).Next(10000, 99999).ToString();
        }
    }
}
