
namespace SOS.AzureStorageAccessLayer
{
    public static class Utility
    {
        public static string ScrapBaseToken(string Token)
        {
            return Token.Substring(2);
        }
    }
}
