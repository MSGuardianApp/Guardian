using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
namespace SOS.AzureStorageAccessLayer
{
    public interface IBlobAccess
    {
        CloudBlobContainer LoadShapeFiles();

        bool UploadShapeFiles(Stream InputStream, String FileName);

        string UploadImage(byte[] Image, string FileName);

        void RemoveImage(string FileName);
    }
}
