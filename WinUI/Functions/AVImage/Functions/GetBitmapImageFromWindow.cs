using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get window icon from process window
        private static async Task<BitmapImage> GetBitmapImageFromWindow(IntPtr windowHandle, int imageWidth, int imageHeight)
        {
            IntPtr iconHandle = IntPtr.Zero;
            try
            {
                //Locks thread when target window is not responding
                iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, (int)GetSetIconFlags.ICON_BIG, 0);
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, (int)GetSetIconFlags.ICON_SMALL, 0);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, (int)GetSetIconFlags.ICON_SMALL2, 0);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = GetClassLongAuto(windowHandle, ClassLongFlags.GCL_HICON);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = GetClassLongAuto(windowHandle, ClassLongFlags.GCL_HICONSM);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    return null;
                }

                //Convert to bitmap
                using (Bitmap bitmap = Icon.FromHandle(iconHandle).ToBitmap())
                {
                    //Convert to bitmap image
                    return await BitmapToBitmapImage(bitmap, imageWidth, imageHeight);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from window: " + ex.Message);
                return null;
            }
            finally
            {
                SafeCloseIcon(ref iconHandle);
            }
        }
    }
}