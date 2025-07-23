using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using WinRT;

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

                //Set control enabled property
                List<Control> ccList = AVVisualTree.FindVisualChildren<Control>(frameworkElement);
                foreach (Control cc in ccList)
                {
                    cc.IsEnabled = enabled;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to set IsEnabled property: " + ex.Message);
            }
        }

        public static bool AVFocus<T>(this T frameworkElement) where T : FrameworkElement
        {
            try
            {
                //Convert to control
                Control frameworkControl = frameworkElement.As<Control>();

                //Focus on control
                bool program = frameworkControl.Focus(FocusState.Programmatic);
                bool pointer = frameworkControl.Focus(FocusState.Pointer);
                bool keyboard = frameworkControl.Focus(FocusState.Keyboard);

                //Return result
                return program && pointer && keyboard;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to focus framework element: " + ex.Message);
                return false;
            }
        }
    }
}