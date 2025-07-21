using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from Path
        public static async Task<BitmapImage> GetBitmapImageFromPath(string sourcePath, int imageWidth, int imageHeight)
        {
            try
            {
                StorageFile fileImage = await StorageFile.GetFileFromPathAsync(sourcePath);
                using (IRandomAccessStream fileStream = await fileImage.OpenAsync(FileAccessMode.Read))
                {
                    return RandomAccessStreamToBitmapImage(fileStream, imageWidth, imageHeight);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from path: " + ex.Message);
                return null;
            }
        }

        //Get BitmapImage from Uri
        public static BitmapImage GetBitmapImageFromUri(Uri sourceUri, int imageWidth, int imageHeight)
        {
            try
            {
                return UriToBitmapImage(sourceUri, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from uri: " + ex.Message);
                return null;
            }
        }
    }
}