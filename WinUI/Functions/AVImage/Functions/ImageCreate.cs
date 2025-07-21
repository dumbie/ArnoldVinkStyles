using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace ArnoldVinkStyles
{
    public partial class AVImage
    {
        //Begin bitmap image
        private static BitmapImage BeginBitmapImage(int imageWidth, int imageHeight)
        {
            try
            {
                //Initializate bitmap image
                BitmapImage bitmapImage = new BitmapImage();

                //Set bitmap image options
                bitmapImage.CreateOptions = BitmapCreateOptions.None;

                //Set bitmap size
                if (imageWidth > 0)
                {
                    bitmapImage.DecodePixelWidth = imageWidth;
                }
                if (imageHeight > 0)
                {
                    bitmapImage.DecodePixelHeight = imageHeight;
                }

                //Return bitmap image
                return bitmapImage;
            }
            catch { }
            return null;
        }

        //End bitmap image
        private static BitmapImage EndBitmapImage(BitmapImage bitmapImage, ref IRandomAccessStream randomAccessStream)
        {
            try
            {
                //Clear memory stream
                if (randomAccessStream != null)
                {
                    randomAccessStream.Dispose();
                }

                //Return bitmap image
                return bitmapImage;
            }
            catch { }
            return null;
        }
    }
}