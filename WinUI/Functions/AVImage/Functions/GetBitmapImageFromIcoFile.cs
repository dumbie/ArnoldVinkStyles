using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        public static async Task<BitmapImage> GetBitmapImageFromIcoFile(string icoFilePath, int imageWidth, int imageHeight)
        {
            List<BitmapFrameOrder> bitmapFrames = new List<BitmapFrameOrder>();
            try
            {
                StorageFile storageFile = await StorageFile.GetFileFromPathAsync(icoFilePath);
                using (IRandomAccessStream fileStream = await storageFile.OpenReadAsync())
                {
                    //Decode ico file stream
                    BitmapDecoder iconBitmapDecoder = await BitmapDecoder.CreateAsync(fileStream);

                    //Add all bitmap frames to list
                    for (uint i = 0; i < iconBitmapDecoder.FrameCount; i++)
                    {
                        BitmapFrame bitmapFrame = await iconBitmapDecoder.GetFrameAsync(i);
                        BitmapFrameOrder bitmapFrameOrder = new BitmapFrameOrder();
                        bitmapFrameOrder.PixelWidth = bitmapFrame.PixelWidth;
                        bitmapFrameOrder.ThumbnailImageStream = await bitmapFrame.GetThumbnailAsync();
                        bitmapFrameOrder.ThumbnailSize = bitmapFrameOrder.ThumbnailImageStream.Size;
                        bitmapFrames.Add(bitmapFrameOrder);
                    }

                    //Sort bitmap frames by size
                    BitmapFrameOrder bitmapFrameLargest = bitmapFrames.OrderBy(x => x.PixelWidth).ThenBy(x => x.ThumbnailSize).LastOrDefault();

                    //Convert image stream to bitmap image
                    return RandomAccessStreamToBitmapImage(bitmapFrameLargest.ThumbnailImageStream, imageWidth, imageHeight);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to load image from ico file: " + ex.Message);
                return null;
            }
            finally
            {
                foreach (BitmapFrameOrder bitmapFrameOrder in bitmapFrames)
                {
                    bitmapFrameOrder.Dispose();
                }
            }
        }

        private class BitmapFrameOrder : IDisposable
        {
            public uint PixelWidth { get; set; }
            public ulong ThumbnailSize { get; set; }
            public ImageStream ThumbnailImageStream { get; set; }
            public void Dispose()
            {
                if (ThumbnailImageStream != null)
                {
                    ThumbnailImageStream.Dispose();
                }
            }
        }
    }
}