using ArnoldVinkCode;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from string path
        private static async Task<BitmapImage> GetBitmapImageFromPath(AVImageFile imageFile, string sourcePath)
        {
            try
            {
                byte[] imageBytes = AVFiles.FileToBytes([sourcePath]);
                return await GetBitmapImageFromBytes(imageFile, imageBytes);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from path: " + ex.Message);
                return null;
            }
        }
    }
}