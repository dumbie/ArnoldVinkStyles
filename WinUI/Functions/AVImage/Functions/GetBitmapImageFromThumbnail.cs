using System;
using System.Diagnostics;
using System.Drawing;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVShell;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from thumbnail
        public static BitmapImage GetBitmapImageFromThumbnail(string filePath, int imageWidth, int imageHeight)
        {
            IntPtr bitmapPointer = IntPtr.Zero;
            try
            {
                //Create shellitem instance
                SHCreateItemFromParsingName(filePath, IntPtr.Zero, typeof(IShellItem2).GUID, out object shellObject);
                if (shellObject == null)
                {
                    Debug.WriteLine("Thumbnail failed to create shellitem instance.");
                    return null;
                }

                //Cast shellitem instance
                IShellItem2 shellItem = (IShellItem2)shellObject;

                //Create thumbnail instance
                IThumbnailCache thumbnailCacheInstance = (IThumbnailCache)Activator.CreateInstance(Type.GetTypeFromCLSID(CLSID_LocalThumbnailCache));
                if (thumbnailCacheInstance == null)
                {
                    Debug.WriteLine("Thumbnail failed to create cache instance.");
                    return null;
                }

                //Get bitmap instance
                thumbnailCacheInstance.GetThumbnail(shellItem, imageWidth, WTS_FLAGS.WTS_EXTRACT, out ISharedBitmap sharedBitmapInstance, out _, out _);
                if (sharedBitmapInstance == null)
                {
                    Debug.WriteLine("Thumbnail failed to create bitmap instance.");
                    return null;
                }

                //Get bitmap pointer
                sharedBitmapInstance.GetSharedBitmap(out bitmapPointer);
                if (bitmapPointer == IntPtr.Zero)
                {
                    Debug.WriteLine("Thumbnail failed bitmap pointer is empty.");
                    return null;
                }

                //Convert pointer to bitmap
                Bitmap sourceBitmap = Bitmap.FromHbitmap(bitmapPointer);

                //Convert to bitmap image
                return BitmapToBitmapImage(ref sourceBitmap, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Thumbnail failed to load: " + filePath + " / " + ex.Message);
                return null;
            }
            finally
            {
                SafeCloseIcon(ref bitmapPointer);
            }
        }
    }
}