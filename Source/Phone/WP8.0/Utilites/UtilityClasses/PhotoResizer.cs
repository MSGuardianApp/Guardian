using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace SOS.Phone.Utilites
{
    public static class PhotoResizer
    {
        public static Stream ReduceSize(Stream originalPhoto)
        {
            Stream resizedPhoto = originalPhoto;
            try
            {
                var bitmapImage = new BitmapImage();
                bitmapImage.CreateOptions = BitmapCreateOptions.DelayCreation;
                bitmapImage.SetSource(originalPhoto);

                int width = bitmapImage.PixelWidth, height = bitmapImage.PixelHeight;
                while (width > 1000 || height > 1000)
                {
                    width = width / 2;
                    height =  height / 2 ;
                }

                MemoryStream memStream = new MemoryStream();
                WriteableBitmap wb = new WriteableBitmap(bitmapImage);
                System.Windows.Media.Imaging.Extensions.SaveJpeg(wb, memStream, width, height, 0, 80);
                memStream.Seek(0, SeekOrigin.Begin);

                resizedPhoto = memStream;
            }
            catch (Exception ex)
            {
                // Absorb any exception and return original photo
            }
            return resizedPhoto;
        }
    }
}
