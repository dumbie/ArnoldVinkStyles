using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using static ArnoldVinkCode.AVSearch;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        public static async Task<BitmapImage> FileToBitmapImage(AVImageFile imageFile)
        {
            //Note: WinUI does not support BitmapSource Freeze so using this as workaround.
            BitmapImage bitmapImageDispatched = null;
            try
            {
                await AVDispatcherInvoke.DispatcherInvoke(imageFile.Dispatcher, async delegate
                {
                    bitmapImageDispatched = await FileToBitmapImageInternal(imageFile);
                });
            }
            catch { }
            return bitmapImageDispatched;
        }

        //Get BitmapImage from file or window
        private static async Task<BitmapImage> FileToBitmapImageInternal(AVImageFile imageFile)
        {
            try
            {
                //Load image from bytes
                if (imageFile.ImageBytes != null)
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromBytes(imageFile.ImageBytes, imageFile.Width, imageFile.Height);
                    if (bitmapImage != null) { return bitmapImage; }
                }

                //Load image from path
                if (imageFile.FilePaths != null)
                {
                    foreach (string filePath in imageFile.FilePaths)
                    {
                        try
                        {
                            //Validate file path
                            if (string.IsNullOrWhiteSpace(filePath)) { continue; }

                            //Adjust file path
                            string filePathLower = filePath.ToLower().Trim();
                            if (filePathLower.Contains("/") && filePathLower.Contains("\\"))
                            {
                                filePathLower = filePathLower.Replace("/", "\\");
                            }
                            Debug.WriteLine("Loading image: " + filePathLower);

                            //Check path exists
                            bool fileExists = File.Exists(filePathLower);
                            bool folderExists = Directory.Exists(filePathLower);

                            if (filePathLower.StartsWith("pack://") || filePathLower.StartsWith("http://") || filePathLower.StartsWith("https://"))
                            {
                                BitmapImage bitmapImage = GetBitmapImageFromUri(new Uri(filePathLower, UriKind.RelativeOrAbsolute), imageFile.Width, imageFile.Height);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (fileExists && filePathLower.EndsWith(".ico"))
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromIcoFile(filePathLower, imageFile.Width, imageFile.Height);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (fileExists && (filePathLower.EndsWith(".exe") || filePathLower.EndsWith(".dll") || filePathLower.EndsWith(".bin")))
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromExecutable(filePathLower, imageFile.IconIndex, imageFile.Width, imageFile.Height);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (fileExists || folderExists)
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromShellFile(filePathLower, imageFile.Width, imageFile.Height, false);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (fileExists)
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromPath(filePathLower, imageFile.Width, imageFile.Height);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (fileExists || folderExists)
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromShellFile(filePathLower, imageFile.Width, imageFile.Height, true);
                                if (bitmapImage != null && bitmapImage.PixelWidth != 0 && bitmapImage.PixelHeight != 0) { return bitmapImage; }
                            }
                            if (imageFile.SearchPaths != null)
                            {
                                string foundImage = Search_Files(new string[] { filePathLower }, imageFile.SearchPaths, false).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(foundImage))
                                {
                                    return await GetBitmapImageFromPath(foundImage, imageFile.Width, imageFile.Height);
                                }
                            }
                        }
                        catch { }
                    }
                }

                //Image not found, load window icon
                if (imageFile.WindowHandle != IntPtr.Zero)
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromWindow(imageFile.WindowHandle, imageFile.Width, imageFile.Height);
                    if (bitmapImage != null) { return bitmapImage; }
                }

                //Image not found, load backup file
                if (!string.IsNullOrWhiteSpace(imageFile.BackupPath))
                {
                    return await GetBitmapImageFromPath(imageFile.BackupPath, imageFile.Width, imageFile.Height);
                }
                else
                {
                    Debug.WriteLine("Failed loading image, no backup file.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed loading image file: " + ex.Message);
                return null;
            }
        }
    }
}