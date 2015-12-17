using System;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Xml.Linq;


namespace SOS.Service.Utility
{
    public static class BingService
    {
        private const string _BingUri = "http://dev.virtualearth.net/REST/v1/Locations/";
        private static string _BingKey = SOS.ConfigManager.Config.BingKey;

        public async static Task<string> GetAddressByPointAsync(string lat, string lng)
        {
            string _UriParameters = string.Format("{0},{1}?o=xml&key={2}", lat, lng, _BingKey);
            _UriParameters = Uri.EscapeUriString(_UriParameters);
            _UriParameters = _BingUri + _UriParameters;

            HttpWebRequest smsReq = (HttpWebRequest)WebRequest.Create(_UriParameters);
            HttpWebResponse smsResp = (HttpWebResponse)(await smsReq.GetResponseAsync());
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(smsResp.GetResponseStream());

            string response = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            smsResp.Close();

            return GetAddressFromBXml(response, lat, lng);
        }

        public static string GetAddressFromBXml(string xmlAddress, string latitude, string longitude)
        {
            string address = string.Empty;
            try
            {
                XDocument xDoc = XDocument.Parse(xmlAddress);
                XNamespace xn = "http://schemas.microsoft.com/search/local/ws/rest/v1";

                var xmlAddr = (from c in xDoc.Descendants(xn + "Address")
                               select c).FirstOrDefault();
                if (xmlAddr != null)
                {
                    string szConfidence = "LOW";
                    var xConfidence = (from c in xDoc.Descendants(xn + "Confidence")
                                       select c).FirstOrDefault();
                    if (xConfidence != null)
                    {
                        szConfidence = xConfidence.Value;
                    }
                    if ((szConfidence.ToUpper() == "HIGH") && xmlAddr.Descendants(xn + "FormattedAddress").Count() > 0)
                    {
                        address = xmlAddr.Descendants(xn + "FormattedAddress").First().Value;
                    }
                    else if (xmlAddr.Descendants(xn + "AddressLine").Count() > 0)
                    {
                        address = xmlAddr.Descendants(xn + "AddressLine").First().Value;
                        if (xmlAddr.Descendants(xn + "Locality").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "Locality").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "AdminDistrict2").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "AdminDistrict2").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "AdminDistrict").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "AdminDistrict").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "CountryRegion").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "CountryRegion").First().Value;
                        }
                        if (xmlAddr.Descendants(xn + "PostalCode").Count() > 0)
                        {
                            address += ", " + xmlAddr.Descendants(xn + "PostalCode").First().Value;
                        }
                    }
                    else if (xmlAddr.Descendants(xn + "FormattedAddress").Count() > 0)
                    { address = xmlAddr.Descendants(xn + "FormattedAddress").First().Value; }
                    else
                    { address = "Lat: " + latitude + "; " + "Long: " + longitude; }
                }
                else
                {
                    address = "Lat: " + latitude + "; " + "Long: " + longitude;//TODO. Format to "0.0000"
                }
            }
            catch
            {
                address = "Lat: " + latitude + "; " + "Long: " + longitude;
            }
            return address;
        }
    }
}
