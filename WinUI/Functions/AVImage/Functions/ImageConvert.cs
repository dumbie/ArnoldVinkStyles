using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Convert Bitmap to RandomAccessStream
        private static IRandomAccessStream BitmapToRandomAccessStream(Bitmap sourceBitmap)
        {
            try
            {
                //Create memory stream
                MemoryStream memoryStream = new MemoryStream();

                //Save bitmap frame
                sourceBitmap.Save(memoryStream, ImageFormat.Png);

                //Set memory stream position
                memoryStream.Position = 0;

                //Return memory stream
                return memoryStream.AsRandomAccessStream();
            }
            catch { }
            return null;
        }

        //Convert Bitmap to BitmapImage
        private static async Task<BitmapImage> BitmapToBitmapImage(Bitmap sourceBitmap, int imageWidth, int imageHeight)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BeginBitmapImage(imageWidth, imageHeight);

                //Save bitmap to random access stream
                IRandomAccessStream randomAccessStream = BitmapToRandomAccessStream(sourceBitmap);

                //Set bitmap image stream source
                await imageToBitmapImage.SetSourceAsync(randomAccessStream);

                //Return application bitmap image
                return EndBitmapImage(imageToBitmapImage, ref randomAccessStream);
            }
            catch { }
            return null;
        }

        //Convert Uri to BitmapImage
        private static BitmapImage UriToBitmapImage(Uri sourceUri, int imageWidth, int imageHeight)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BeginBitmapImage(imageWidth, imageHeight);
                IRandomAccessStream randomAccessStream = null;

                //Set bitmap image uri source
                imageToBitmapImage.UriSource = sourceUri;

                //Return application bitmap image
                return EndBitmapImage(imageToBitmapImage, ref randomAccessStream);
            }
            catch { }
            return null;
        }

        //Convert Bytes to BitmapImage
        private static async Task<BitmapImage> BytesToBitmapImage(byte[] sourceBytes, int imageWidth, int imageHeight)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BeginBitmapImage(imageWidth, imageHeight);

                //Save bytes to memorystream
                MemoryStream memoryStream = new MemoryStream(sourceBytes);
                IRandomAccessStream randomAccessStream = memoryStream.AsRandomAccessStream();

                //Set bitmap image stream source
                await imageToBitmapImage.SetSourceAsync(randomAccessStream);

                //Return application bitmap image
                return EndBitmapImage(imageToBitmapImage, ref randomAccessStream);
            }
            catch { }
            return null;
        }

        //Convert RandomAccessStream to BitmapImage
        private static async Task<BitmapImage> RandomAccessStreamToBitmapImage(IRandomAccessStream sourceStream, int imageWidth, int imageHeight)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BeginBitmapImage(imageWidth, imageHeight);

                //Set bitmap image stream source
                await imageToBitmapImage.SetSourceAsync(sourceStream);

                //Return application bitmap image
                return EndBitmapImage(imageToBitmapImage, ref sourceStream);
            }
            catch { }
            return null;
        }
    }
}