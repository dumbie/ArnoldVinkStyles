using System;
using System.Diagnostics;
using System.Drawing;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVInteropDll;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get window icon from process window
        public static BitmapImage GetBitmapImageFromWindow(IntPtr windowHandle, int imageWidth, int imageHeight)
        {
            IntPtr iconHandle = IntPtr.Zero;
            try
            {
                int GCL_HICON = -14;
                int GCL_HICONSM = -34;
                int ICON_SMALL = 0;
                int ICON_BIG = 1;
                int ICON_SMALL2 = 2;

                //Locks thread when target window is not responding
                iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, ICON_BIG, 0);
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, ICON_SMALL, 0);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = SendMessage(windowHandle, WindowMessages.WM_GETICON, ICON_SMALL2, 0);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = GetClassLongAuto(windowHandle, GCL_HICON);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    iconHandle = GetClassLongAuto(windowHandle, GCL_HICONSM);
                }
                if (iconHandle == IntPtr.Zero)
                {
                    return null;
                }

                //Convert to bitmap
                Bitmap bitmap = Icon.FromHandle(iconHandle).ToBitmap();

                //Convert to bitmap image
                return BitmapToBitmapImage(ref bitmap, imageWidth, imageHeight);
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