using System;
using System.Diagnostics;
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
        private static IRandomAccessStream BitmapToRandomAccessStream(Bitmap imageBitmap)
        {
            try
            {
                //Create memory stream
                MemoryStream memoryStream = new MemoryStream();

                //Save bitmap frame
                imageBitmap.Save(memoryStream, ImageFormat.Png);

                //Set memory stream position
                memoryStream.Position = 0;

                //Return memory stream
                return memoryStream.AsRandomAccessStream();
            }
            catch { }
            return null;
        }

        //Convert Uri to BitmapImage
        private static BitmapImage GetBitmapImageFromUri(AVImageFile imageFile, Uri imageUri)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BitmapImageCreate(imageFile);

                //Set bitmap image uri source
                BitmapImageSet(imageFile, imageToBitmapImage, imageUri);

                //Return application bitmap image
                return imageToBitmapImage;
            }
            catch { }
            return null;
        }

        //Convert Bitmap to BitmapImage
        private static async Task<BitmapImage> GetBitmapImageFromBitmap(AVImageFile imageFile, Bitmap imageBitmap)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BitmapImageCreate(imageFile);

                //Save bitmap to random access stream
                using (IRandomAccessStream randomAccessStream = BitmapToRandomAccessStream(imageBitmap))
                {
                    //Set bitmap image stream source
                    await BitmapImageSet(imageFile, imageToBitmapImage, randomAccessStream);
                }

                //Return application bitmap image
                return imageToBitmapImage;
            }
            catch { }
            return null;
        }

        //Convert Bytes to BitmapImage
        private static async Task<BitmapImage> GetBitmapImageFromBytes(AVImageFile imageFile, byte[] imageBytes)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BitmapImageCreate(imageFile);

                //Save bytes to random access stream
                using (IRandomAccessStream randomAccessStream = new MemoryStream(imageBytes).AsRandomAccessStream())
                {
                    //Set bitmap image stream source
                    await BitmapImageSet(imageFile, imageToBitmapImage, randomAccessStream);
                }

                //Return application bitmap image
                return imageToBitmapImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from bytes: " + ex.Message);
                return null;
            }
        }

        //Convert RandomAccessStream to BitmapImage
        private static async Task<BitmapImage> GetBitmapImageFromStream(AVImageFile imageFile, IRandomAccessStream randomAccessStream)
        {
            try
            {
                //Prepare application bitmap image
                BitmapImage imageToBitmapImage = BitmapImageCreate(imageFile);

                //Set bitmap image stream source
                await BitmapImageSet(imageFile, imageToBitmapImage, randomAccessStream);

                //Clear memory stream
                if (randomAccessStream != null)
                {
                    randomAccessStream.Dispose();
                }

                //Return application bitmap image
                return imageToBitmapImage;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from stream: " + ex.Message);
                return null;
            }
        }
    }
}