using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Create bitmap image
        private static BitmapImage BitmapImageCreate(AVImageFile imageFile)
        {
            BitmapImage bitmapImage = null;
            try
            {
                AVDispatcherInvoke.DispatcherInvoke(imageFile.Dispatcher, delegate
                {
                    //Create bitmap image (ui thread to prevent COMException)
                    bitmapImage = new BitmapImage();

                    //Set bitmap options
                    bitmapImage.CreateOptions = BitmapCreateOptions.None;

                    //Set bitmap size
                    if (imageFile.Width > 0)
                    {
                        bitmapImage.DecodePixelWidth = imageFile.Width;
                    }
                    if (imageFile.Height > 0)
                    {
                        bitmapImage.DecodePixelHeight = imageFile.Height;
                    }
                });
            }
            catch { }
            return bitmapImage;
        }

        //Check bitmap image
        private static bool BitmapImageCheck(AVImageFile imageFile, BitmapImage bitmapImage)
        {
            bool bitmapValid = true;
            try
            {
                //Check bitmap image is null
                if (bitmapImage == null)
                {
                    return false;
                }

                //Check bitmap image size
                AVDispatcherInvoke.DispatcherInvoke(imageFile.Dispatcher, delegate
                {
                    //Debug.WriteLine("Checking bitmap image: " + bitmapImage.PixelWidth + "w " + bitmapImage.PixelHeight + "h");
                    if (bitmapImage.PixelWidth <= 0)
                    {
                        bitmapValid = false;
                    }
                    else if (bitmapImage.PixelHeight <= 0)
                    {
                        bitmapValid = false;
                    }
                });
            }
            catch { }
            return bitmapValid;
        }

        //Set bitmap image source
        private static async Task<bool> BitmapImageSet(AVImageFile imageFile, BitmapImage bitmapImage, IRandomAccessStream randomAccessStream)
        {
            try
            {
                await AVDispatcherInvoke.DispatcherInvoke(imageFile.Dispatcher, async delegate
                {
                    await bitmapImage.SetSourceAsync(randomAccessStream);
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Set bitmap uri source
        private static bool BitmapImageSet(AVImageFile imageFile, BitmapImage bitmapImage, Uri uri)
        {
            try
            {
                AVDispatcherInvoke.DispatcherInvoke(imageFile.Dispatcher, delegate
                {
                    bitmapImage.UriSource = uri;
                });
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}