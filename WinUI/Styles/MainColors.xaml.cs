using System;
using System.Diagnostics;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ArnoldVinkStyles
{
    public partial class MainColors : ResourceDictionary
    {
        //Change application accent color
        public static void ChangeApplicationAccentColor(string colorLightHex)
        {
            try
            {
                //FixStyle UWP does not support DynamicResource by default
                Debug.WriteLine("Changing the application accent color.");

                SolidColorBrush targetSolidColorBrushLight = new SolidColorBrush(Color.FromArgb(Convert.ToByte(colorLightHex.Substring(1, 2), 16), Convert.ToByte(colorLightHex.Substring(3, 2), 16), Convert.ToByte(colorLightHex.Substring(5, 2), 16), Convert.ToByte(colorLightHex.Substring(7, 2), 16)));
                Application.Current.Resources["ApplicationAccentLightColor"] = targetSolidColorBrushLight.Color;
                Application.Current.Resources["ApplicationAccentLightBrush"] = targetSolidColorBrushLight;
                Debug.WriteLine("Accent light color: " + targetSolidColorBrushLight.Color);

                SolidColorBrush targetSolidColorBrushDim = AVColors.AdjustColorBrightness(targetSolidColorBrushLight, 0.80);
                Application.Current.Resources["ApplicationAccentDimColor"] = targetSolidColorBrushDim.Color;
                Application.Current.Resources["ApplicationAccentDimBrush"] = targetSolidColorBrushDim;
                Debug.WriteLine("Accent dim color: " + targetSolidColorBrushDim.Color);

                SolidColorBrush targetSolidColorBrushDark = AVColors.AdjustColorBrightness(targetSolidColorBrushLight, 0.50);
                Application.Current.Resources["ApplicationAccentDarkColor"] = targetSolidColorBrushDark.Color;
                Application.Current.Resources["ApplicationAccentDarkBrush"] = targetSolidColorBrushDark;
                Debug.WriteLine("Accent dark color: " + targetSolidColorBrushDark.Color);
            }
            catch { }
        }
    }
}