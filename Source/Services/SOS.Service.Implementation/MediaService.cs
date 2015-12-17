using System;
using System.IO;
using SOS.Service.Interfaces;
using entities = SOS.AzureStorageAccessLayer.Entities;

namespace SOS.Service.Implementation
{
    public class MediaService : IMediaService
    {
        public void SaveTeaseImage(Stream imgStream)
        {
            string path = @"E:\uploadSync\" + DateTime.Now + ".jpg";
            var filestrm = new FileStream(path, FileMode.Create);
            imgStream.CopyTo(filestrm);
            imgStream.Close();
        }
    }
}