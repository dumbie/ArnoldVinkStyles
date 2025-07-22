using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ArnoldVinkStyles
{
    public static partial class AVExtensions
    {
        /// <summary>
        /// Extension to be able to disable Grid and StackPanel elements.
        /// </summary>
        public static void AvIsEnabled<T>(this T frameworkElement, bool enabled, double? opacity) where T : FrameworkElement
        {
            try
            {
                //Update framework element opacity
                if (opacity != null)
                {
                    frameworkElement.Opacity = (double)opacity;
                }

                //Set content control enabled property
                List<ContentControl> ccList = AVVisualTree.FindVisualChildren<ContentControl>(frameworkElement);
                foreach (ContentControl cc in ccList)
                {
                    cc.IsEnabled = enabled;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set IsEnabled property: " + ex.Message);
            }
        }
    }
}