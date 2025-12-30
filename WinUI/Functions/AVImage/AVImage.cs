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
        //Get BitmapImage from file or window
        //Note: WinUI does not support BitmapSource Freeze so using dispatcher is required
        //Note: WinUI BitmapImage needs to be initialized in UI thread to prevent COMException
        public static async Task<BitmapImage> FileToBitmapImage(AVImageFile imageFile)
        {
            try
            {
                //Load image from path
                if (imageFile.FilePaths != null && imageFile.FilePaths.Any())
                {
                    foreach (string filePath in imageFile.FilePaths)
                    {
                        try
                        {
                            //Validate file path
                            if (string.IsNullOrWhiteSpace(filePath)) { continue; }

                            //Adjust file path
                            string filePathTrim = filePath.Trim();
                            if (filePathTrim.Contains("/") && filePathTrim.Contains("\\"))
                            {
                                filePathTrim = filePathTrim.Replace("/", "\\");
                            }
                            string filePathLower = filePathTrim.ToLower();

                            //Check file exists
                            bool fileExists = File.Exists(filePathLower);
                            bool folderExists = Directory.Exists(filePathLower);

                            //Write debug line
                            Debug.WriteLine("Loading image: " + filePathLower + " " + fileExists + "/" + folderExists);

                            if (filePathLower.Contains("://"))
                            {
                                BitmapImage bitmapImage = GetBitmapImageFromUri(imageFile, new Uri(filePathTrim, UriKind.RelativeOrAbsolute));
                                if (bitmapImage != null) { return bitmapImage; } //Uri can't be checked
                            }
                            if (fileExists && filePathLower.EndsWith(".ico"))
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromIcoFile(imageFile, filePathLower);
                                if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                            }
                            if (fileExists && (filePathLower.EndsWith(".exe") || filePathLower.EndsWith(".dll") || filePathLower.EndsWith(".bin")))
                            {
                                BitmapImage bitmapImage = await GetBitmapImageFromExecutable(imageFile, filePathLower, imageFile.IconIndex);
                                if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                            }
                            if (imageFile.UseThumbnail)
                            {
                                if (fileExists || folderExists)
                                {
                                    BitmapImage bitmapImageThumb = await GetBitmapImageFromShellFile(imageFile, filePathLower, false);
                                    if (BitmapImageCheck(imageFile, bitmapImageThumb)) { return bitmapImageThumb; }

                                    BitmapImage bitmapImageIcon = await GetBitmapImageFromShellFile(imageFile, filePathLower, true);
                                    if (BitmapImageCheck(imageFile, bitmapImageIcon)) { return bitmapImageIcon; }
                                }
                            }
                            else
                            {
                                if (fileExists)
                                {
                                    BitmapImage bitmapImage = await GetBitmapImageFromPath(imageFile, filePathLower);
                                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                                }
                            }
                            if (imageFile.SearchPaths != null && imageFile.SearchPaths.Any())
                            {
                                string foundImage = Search_Files([filePathLower], imageFile.SearchPaths, false).FirstOrDefault();
                                if (!string.IsNullOrWhiteSpace(foundImage))
                                {
                                    BitmapImage bitmapImage = await GetBitmapImageFromPath(imageFile, foundImage);
                                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                                }
                            }
                        }
                        catch { }
                    }
                }

                //Load image from uri
                if (imageFile.ImageUri != null)
                {
                    BitmapImage bitmapImage = GetBitmapImageFromUri(imageFile, imageFile.ImageUri);
                    if (bitmapImage != null) { return bitmapImage; } //Uri can't be checked
                }

                //Load image from file type
                if (!string.IsNullOrWhiteSpace(imageFile.FileType))
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromFileType(imageFile, imageFile.FileType);
                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                }

                //Load image from window
                if (imageFile.WindowHandle != IntPtr.Zero)
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromWindow(imageFile, imageFile.WindowHandle);
                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                }

                //Load image from bitmap
                if (imageFile.ImageBitmap != null)
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromBitmap(imageFile, imageFile.ImageBitmap);
                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                }

                //Load image from bytes
                if (imageFile.ImageBytes != null && imageFile.ImageBytes.Any())
                {
                    BitmapImage bitmapImage = await GetBitmapImageFromBytes(imageFile, imageFile.ImageBytes);
                    if (BitmapImageCheck(imageFile, bitmapImage)) { return bitmapImage; }
                }

                //Image not found, load backup file
                if (!string.IsNullOrWhiteSpace(imageFile.BackupPath))
                {
                    return await GetBitmapImageFromPath(imageFile, imageFile.BackupPath);
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