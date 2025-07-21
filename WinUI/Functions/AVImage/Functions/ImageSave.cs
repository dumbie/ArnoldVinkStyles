using ArnoldVinkCode;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Save FrameworkElement to PNG file
        public static async Task<bool> FrameworkElementToFile(FrameworkElement frameworkElement, string targetPath, bool overwrite)
        {
            try
            {
                if (overwrite || !File.Exists(targetPath))
                {
                    //Create render target bitmap
                    RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap();
                    await renderTargetBitmap.RenderAsync(frameworkElement);
                    IBuffer renderTargetBitmapPixels = await renderTargetBitmap.GetPixelsAsync();

                    //Encode bitmap to png bytes
                    using (MemoryStream imageMemoryStream = new MemoryStream())
                    {
                        uint bitmapWidth = (uint)frameworkElement.ActualWidth;
                        uint bitmapHeight = (uint)frameworkElement.ActualHeight;
                        BitmapEncoder bitmapEncoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, imageMemoryStream.AsRandomAccessStream());
                        bitmapEncoder.SetPixelData(BitmapPixelFormat.Bgra8, BitmapAlphaMode.Ignore, bitmapWidth, bitmapHeight, 96, 96, renderTargetBitmapPixels.ToArray());
                        await bitmapEncoder.FlushAsync();

                        //Write bytes to file
                        return AVFiles.BytesToFile(targetPath, imageMemoryStream.ToArray());
                    }
                }
            }
            catch { }
            return false;
        }
    }
}