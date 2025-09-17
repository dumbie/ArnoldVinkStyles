using System;
using System.Windows.Media;

namespace ArnoldVinkStyles
{
    public partial class AVColorConverters
    {
        /// <summary>
        /// Convert hex color string to SolidColorBrush
        /// Usage: HexToSolidColorBrush("#FF000000");
        /// </summary>
        public static SolidColorBrush HexToSolidColorBrush(string colorHex)
        {
            try
            {
                //Check if hex contains alpha channel
                if (colorHex.Length == 9)
                {
                    return new SolidColorBrush(Color.FromArgb(Convert.ToByte(colorHex.Substring(1, 2), 16), Convert.ToByte(colorHex.Substring(3, 2), 16), Convert.ToByte(colorHex.Substring(5, 2), 16), Convert.ToByte(colorHex.Substring(7, 2), 16)));
                }
                else
                {
                    return new SolidColorBrush(Color.FromArgb(255, Convert.ToByte(colorHex.Substring(1, 2), 16), Convert.ToByte(colorHex.Substring(3, 2), 16), Convert.ToByte(colorHex.Substring(5, 2), 16)));
                }
            }
            catch
            {
                return null;
            }
        }
    }
}