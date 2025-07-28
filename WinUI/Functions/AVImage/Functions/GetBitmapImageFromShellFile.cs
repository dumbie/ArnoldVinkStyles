using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;
using static ArnoldVinkCode.AVShell;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from shell file
        private static async Task<BitmapImage> GetBitmapImageFromShellFile(string filePath, int imageWidth, int imageHeight, bool iconOnly)
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
                shellItem.GetImage(bitmapSize, extractFlags, out bitmapPointer);

                //Check bitmap pointer
                if (bitmapPointer == IntPtr.Zero)
                {
                    Debug.WriteLine("Shell file failed bitmap pointer is empty.");
                    return null;
                }

                //Convert pointer to bitmap
                using (Bitmap sourceBitmap = Bitmap.FromHbitmap(bitmapPointer))
                {
                    //Make bitmap transparent
                    sourceBitmap.MakeTransparent(); //Fix transparency issue

                    //Convert to bitmap image
                    return await BitmapToBitmapImage(sourceBitmap, imageWidth, imageHeight);
                }
            }
            catch (Exception ex)
            {
                //Fix WTS_E_FAILEDEXTRACTION error when loading files from OneDrive
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