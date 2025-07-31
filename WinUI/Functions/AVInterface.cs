using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;

namespace ArnoldVinkStyles
{
    public partial class AVInterface
    {
        /// <summary>
        /// Get currently focused framework element
        /// Note: Must be run on UI thread.
        /// </summary>
        public static FrameworkElement GetFocusedFrameworkElement()
        {
            try
            {
                return (FrameworkElement)FocusManager.GetFocusedElement();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get focused FrameworkElement: " + ex.Message);
                return null;
            }
        }
    }
}