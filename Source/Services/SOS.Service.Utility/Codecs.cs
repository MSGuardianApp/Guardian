using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace SOS.Service.Utility
{
    public static class SOSCodecs
    {
        public static UTF8Encoding UTF8Encoder = new UTF8Encoding(true, true);
        private const string beginCert = "-----BEGIN CERTIFICATE-----\\n";
        private const string endCert = "\\n-----END CERTIFICATE-----\\n";


        public static byte[] UrlDecode(string encodedSegment)
        {
            
            string s = encodedSegment;
            s = s.Replace('-', '+'); // 62nd char of encoding
            s = s.Replace('_', '/'); // 63rd char of encoding
            switch (s.Length % 4) // Pad with trailing '='s
            {
                case 0: break; // No pad chars in this case
                case 2: s += "=="; break; // Two pad chars
                case 3: s += "="; break; // One pad char
                default: throw new System.Exception("Illegal base64url string");
            }
            return Convert.FromBase64String(s); // Standard base64 decoder
        }

        public static string UrlEncode(byte[] arg)
        {
            string s = Convert.ToBase64String(arg); // Standard base64 encoder
            s = s.Split('=')[0]; // Remove any trailing '='s
            s = s.Replace('+', '-'); // 62nd char of encoding
            s = s.Replace('/', '_'); // 63rd char of encoding
            return s;
        }

        public static object Deserialize(string Base64EncodedJSONText, Type TypeInfo)
        {
            string envelopeText = Base64EncodedJSONText;
            byte[] envelopeData = SOSCodecs.UrlDecode(envelopeText);
            using (MemoryStream memoryStream = new MemoryStream(envelopeData))
            {
                return new DataContractJsonSerializer(TypeInfo).ReadObject(memoryStream);
            }

        }

        public static string Serialize(object Object, Type TypeInfo)
        {
            var serializer = new DataContractJsonSerializer(TypeInfo);
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.WriteObject(stream, Object);
                return Encoding.Default.GetString(stream.ToArray());
            } 
        }

        public static byte[][] getGoogleCertBytes()
        {
            // The request will be made to the authentication server.
            WebRequest request = WebRequest.Create("https://www.googleapis.com/oauth2/v1/certs");
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string responseFromServer = reader.ReadToEnd();
            String[] split = responseFromServer.Split(':');

            // There are two certificates returned from Google
            byte[][] certBytes = new byte[2][];
            int index = 0;
            UTF8Encoding utf8 = new UTF8Encoding();
            for (int i = 0; i < split.Length; i++)
            {
                if (split[i].IndexOf(beginCert) > 0)
                {
                    int startSub = split[i].IndexOf(beginCert);
                    int endSub = split[i].IndexOf(endCert) + endCert.Length;
                    certBytes[index] = utf8.GetBytes(split[i].Substring(startSub, endSub).Replace("\\n", "\n"));
                    index++;
                }
            }
            return certBytes;
        }

    }
}
