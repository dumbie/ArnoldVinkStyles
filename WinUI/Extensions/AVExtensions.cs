using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
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

        public static bool AVFocusable<T>(this T frameworkElement) where T : FrameworkElement
        {
            try
            {
                //Check visibility
                if (frameworkElement.Visibility != Visibility.Visible)
                {
                    return false;
                }

                //Convert to control
                Control frameworkControl = frameworkElement.As<Control>();

                //Check if disabled
                if (!frameworkControl.IsEnabled)
                {
                    return false;
                }

                //Check tab stop
                if (!frameworkControl.IsTabStop)
                {
                    return false;
                }

                //Return result
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to check if is focusable: " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Note: ListView must be visible to successfully extract ListViewItem
        /// </summary>
        public static ListViewItem AVGetListViewItem<T>(this T targetListView, object targetItem) where T : ListView
        {
            try
            {
                //Alternative for ItemContainerGenerator.ContainerFromItem
                //Check targetListView RenderSize?

                //Update layout
                targetListView.UpdateLayout();

                //Get all list view items
                List<ListViewItem> listViewItems = AVVisualTree.FindVisualChildren<ListViewItem>(targetListView);

                //Search for target item
                return listViewItems.Where(x => x.Content == targetItem).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get ListViewItem: " + ex.Message);
                return null;
            }
        }

        //Check if framework element is visible for user
        public static bool AVVisibleUser<T>(this T elementTarget, FrameworkElement elementParent) where T : FrameworkElement
        {
            try
            {
                //Check framework element null
                if (elementTarget == null)
                {
                    //Debug.WriteLine("Framework element target is null.");
                    return false;
                }
                if (elementParent == null)
                {
                    //Debug.WriteLine("Framework element parent is null.");
                    return false;
                }

                //Check framework element visibility
                if (elementTarget.Visibility != Visibility.Visible)
                {
                    //Debug.WriteLine("Framework element target is not visible.");
                    return false;
                }
                if (elementParent.Visibility != Visibility.Visible)
                {
                    //Debug.WriteLine("Framework element parent is not visible.");
                    return false;
                }

                //Set and check render size
                Rect rectTarget = new Rect();
                rectTarget.Width = elementTarget.RenderSize.Width;
                rectTarget.Height = elementTarget.RenderSize.Height;
                if (rectTarget.Height == 0 || rectTarget.Width == 0)
                {
                    //Debug.WriteLine("Framework element target is not rendered.");
                    return false;
                }
                Rect rectParent = new Rect();
                rectParent.Width = elementParent.RenderSize.Width;
                rectParent.Height = elementParent.RenderSize.Height;
                if (rectParent.Height == 0 || rectParent.Width == 0)
                {
                    //Debug.WriteLine("Framework element parent is not rendered.");
                    return false;
                }

                //Get rectangle transform bounds
                GeneralTransform generalTransform = elementTarget.TransformToVisual(elementParent);
                Rect rectBounds = generalTransform.TransformBounds(rectTarget);

                //Check if rectangles intersect within
                return (rectParent.Left <= rectBounds.Right) && (rectParent.Right >= rectBounds.Left) && (rectParent.Top <= rectBounds.Bottom) && (rectParent.Bottom >= rectBounds.Top);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed checking element user visibility: " + ex.Message);
                return false;
            }
        }
    }
}