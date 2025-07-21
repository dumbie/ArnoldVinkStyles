using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Get BitmapImage from thumbnail
        public async static Task<BitmapImage> GetBitmapImageFromThumbnail(string filePath, int imageWidth, int imageHeight)
        {
            StorageItemThumbnail storageItemThumbnail = null;
            try
            {
                //Check if path is file or folder
                bool pathIsFolder = File.GetAttributes(filePath).HasFlag(System.IO.FileAttributes.Directory);
                if (pathIsFolder)
                {
                    StorageFolder storageFolder = await StorageFolder.GetFolderFromPathAsync(filePath);
                    storageItemThumbnail = await storageFolder.GetThumbnailAsync(ThumbnailMode.SingleItem, (uint)imageWidth, ThumbnailOptions.ResizeThumbnail);
                }
                else
                {
                    StorageFile storageFile = await StorageFile.GetFileFromPathAsync(filePath);
                    storageItemThumbnail = await storageFile.GetThumbnailAsync(ThumbnailMode.SingleItem, (uint)imageWidth, ThumbnailOptions.ResizeThumbnail);
                }

                //Convert image stream to bitmap image
                return RandomAccessStreamToBitmapImage(storageItemThumbnail, imageWidth, imageHeight);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from thumbnail: " + ex.Message);
                return null;
            }
            finally
            {
                if (storageItemThumbnail != null)
                {
                    storageItemThumbnail.Dispose();
                }
            }
        }
    }
}