using System;
using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using static ArnoldVinkStyles.AVVisualTree;

namespace ArnoldVinkStyles
{
    public partial class AVInterface
    {
        /// <summary>
        /// Get and return clicked ListViewItem object
        /// </summary>
        public static T GetClickedListViewItemObject<T>(RoutedEventArgs e) where T : class
        {
            try
            {
                if (e.OriginalSource is ListViewItem)
                {
                    return (T)((ListViewItem)e.OriginalSource).Content;
                }
                else
                {
                    return (T)((FrameworkElement)e.OriginalSource).DataContext;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get clicked ListViewItem object: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get and return clicked ListViewItem container
        /// </summary>
        public static ListViewItem GetClickedListViewItemContainer(RoutedEventArgs e)
        {
            try
            {
                if (e.OriginalSource is ListViewItem)
                {
                    return (ListViewItem)e.OriginalSource;
                }
                else
                {
                    return FindVisualParent<ListViewItem>((FrameworkElement)e.OriginalSource);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get clicked ListViewItem container: " + ex.Message);
                return null;
            }
        }

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