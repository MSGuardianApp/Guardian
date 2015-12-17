using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using SOS.ConfigManager;
namespace SOS.AzureStorageAccessLayer
{
    public class BlobAccess
    {
        public BlobAccess()
        {
            _StorageAccount = Microsoft.WindowsAzure.Storage.CloudStorageAccount.Parse(Config.BlobConnectionString);
            _BlobClient = StorageAccount.CreateCloudBlobClient();
        }
        private Microsoft.WindowsAzure.Storage.CloudStorageAccount _StorageAccount = null;

        internal Microsoft.WindowsAzure.Storage.CloudStorageAccount StorageAccount
        {
            get { return _StorageAccount; }
            private set { _StorageAccount = value; }
        }

        private CloudBlobClient _BlobClient = null;

        internal CloudBlobClient BlobClient
        {
            get { return _BlobClient; }
            private set { _BlobClient = value; }
        }

        private CloudBlobContainer _imageContainer = null;
       
        internal bool IsImageContainerLoaded = false;
        internal void LoadImagesContainer()
        {
            try
            {
                _imageContainer = _BlobClient.GetContainerReference("imagescontainer");

                _imageContainer.CreateIfNotExists();

                _imageContainer.SetPermissions(new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    });
                IsImageContainerLoaded = true;
            }
            catch (Exception ex)
            {
            }
        }

        public CloudBlobContainer LoadShapeFiles()
        {
            CloudBlobContainer _shapeContainer = null;
            try
            {
                _shapeContainer = _BlobClient.GetContainerReference("shapes");
                _shapeContainer.CreateIfNotExists();               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _shapeContainer;
        }


        public bool UploadShapeFiles(Stream InputStream,String FileName)
        {
            bool flag = false;
            try
            {
                CloudBlobContainer _shapeContainer = LoadShapeFiles();
                CloudBlockBlob blockBlob = _shapeContainer.GetBlockBlobReference(FileName);
                blockBlob.UploadFromStream(InputStream);
                flag = true;
            }
            catch (Exception)
            {
                
            }
            return flag;
        }



        public string UploadImage(byte[] Image, string FileName)
        {
            if (!IsImageContainerLoaded)
                LoadImagesContainer();
            CloudBlockBlob blockBlob = _imageContainer.GetBlockBlobReference(FileName);
            using (Stream s = new MemoryStream(Image))
            {
                blockBlob.UploadFromStream(s);
            }
            return blockBlob.Uri.AbsoluteUri;
        }

        public void RemoveImage(string FileName)
        {
            if (!IsImageContainerLoaded)
                LoadImagesContainer();
            CloudBlockBlob blockBlob = _imageContainer.GetBlockBlobReference(FileName);
            blockBlob.DeleteIfExists();
            
        }
    }
}
