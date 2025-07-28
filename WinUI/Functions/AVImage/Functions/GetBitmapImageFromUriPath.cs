using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from Path
        private static async Task<BitmapImage> GetBitmapImageFromPath(string sourcePath, int imageWidth, int imageHeight)
        {
            try
            {
                byte[] imageBytes = AVFiles.FileToBytes([sourcePath]);
                return await BytesToBitmapImage(imageBytes, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from path: " + ex.Message);
                return null;
            }
        }

        //Get BitmapImage from Uri
        private static BitmapImage GetBitmapImageFromUri(Uri sourceUri, int imageWidth, int imageHeight)
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