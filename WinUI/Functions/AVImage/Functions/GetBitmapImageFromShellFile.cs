using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVShell;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from shell file
        private static BitmapImage GetBitmapImageFromShellFile(string filePath, int imageWidth, int imageHeight, bool iconOnly)
        {
            IntPtr bitmapPointer = IntPtr.Zero;
            try
            {
                //Create shellitem instance
                SHCreateItemFromParsingName(filePath, IntPtr.Zero, typeof(IShellItemImageFactory).GUID, out object shellObject);
                if (shellObject == null)
                {
                    Debug.WriteLine("Shell file failed to create shellitem image instance.");
                    return null;
                }

                //Cast shellitem instance
                IShellItemImageFactory shellItem = (IShellItemImageFactory)shellObject;

                //Set bitmap target size
                WindowSize bitmapSize = new WindowSize();
                bitmapSize.cx = imageWidth;
                bitmapSize.cy = imageHeight;

                //Set bitmap flags
                SIIGBF extractFlags = SIIGBF.SIIGBF_BIGGERSIZEOK | (iconOnly ? SIIGBF.SIIGBF_ICONONLY : SIIGBF.SIIGBF_THUMBNAILONLY);

                //Get bitmap pointer
                try
                {
                    shellItem.GetImage(bitmapSize, extractFlags, out bitmapPointer);
                }
                catch (COMException ex)
                {
                    //Fix WTS_E_FAILEDEXTRACTION error when loading files from OneDrive
                    Debug.WriteLine("Shell file failed to extract: " + (WTS_EXCEPTIONS)ex.HResult + " / " + filePath);
                }

                //Check bitmap pointer
                if (bitmapPointer == IntPtr.Zero)
                {
                    Debug.WriteLine("Shell file failed bitmap pointer is empty.");
                    return null;
                }

                //Convert pointer to bitmap
                Bitmap sourceBitmap = Bitmap.FromHbitmap(bitmapPointer);
                sourceBitmap.MakeTransparent(); //Fix transparency issue

                //Convert to bitmap image
                return BitmapToBitmapImage(ref sourceBitmap, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Shell file failed to load: " + filePath + " / " + ex.Message);
                return null;
            }
            finally
            {
                SafeCloseIcon(ref bitmapPointer);
            }
        }
    }
}