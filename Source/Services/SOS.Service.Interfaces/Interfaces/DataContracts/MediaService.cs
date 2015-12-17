namespace SOS.Service.Interfaces.DataContracts
{
    public class Media
    {
        public string MediaID { get; set; }

        public string BlobURI { get; set; }

        public string Lat { get; set; }

        public string Long { get; set; }

        public MediaType Type { get; set; }

        public long TimeStamp { get; set; }

    }

    public enum MediaType
    {
        Image,
        Video
    }
}
