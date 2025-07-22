using ArnoldVinkCode;
using System;
using System.Diagnostics;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from Path
        public static BitmapImage GetBitmapImageFromPath(string sourcePath, int imageWidth, int imageHeight)
        {
            try
            {
                byte[] imageBytes = AVFiles.FileToBytes([sourcePath]);
                return BytesToBitmapImage(imageBytes, imageWidth, imageHeight);
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