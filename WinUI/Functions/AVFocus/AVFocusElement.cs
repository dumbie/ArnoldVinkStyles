using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace ArnoldVinkStyles
{
    public partial class AVFocus
    {
        /// <summary>
        /// Get currently focused framework element
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

        /// <summary>
        /// Get currently focused ListView
        /// </summary>
        public static ListView GetFocusedListView()
        {
            ListView focusedListView = null;
            try
            {
                FrameworkElement frameworkElement = GetFocusedFrameworkElement();
                if (frameworkElement != null)
                {
                    if (frameworkElement is ListView)
                    {
                        focusedListView = (ListView)frameworkElement;
                    }
                    else if (frameworkElement is ListViewItem)
                    {
                        focusedListView = AVVisualTree.FindVisualParent<ListView>(frameworkElement);
                    }
                }
            }
            catch { }
            return focusedListView;
        }

        /// <summary>
        /// Check if focused on framework element
        /// </summary>
        public static async Task CheckFocusFrameworkElement()
        {
            try
            {
                //Get focused element
                FrameworkElement focusedElement = GetFocusedFrameworkElement();

                //Check currently focused element
                if (focusedElement == null)
                {
                    await FocusFrameworkElement(FocusNavigationDirection.Next);
                    Debug.WriteLine("Keyboard check not focused, focusing on next element.");
                }
                else
                {
                    //Check if focused element is container
                    if (focusedElement is Panel || focusedElement is ItemsControl || focusedElement is ScrollViewer)
                    {
                        await FocusFrameworkElement(FocusNavigationDirection.Next);
                        Debug.WriteLine("Container is focused, focusing on next element.");
                    }
                    else
                    {
                        await FocusFrameworkElement(focusedElement);
                        Debug.WriteLine("Keyboard check focused on: " + focusedElement);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed checking focus: " + ex.Message);
            }
        }

        /// <summary>
        /// Focus on direction framework element
        /// </summary>
        public static async Task FocusFrameworkElement(FocusNavigationDirection focusDirection)
        {
            try
            {
                //Get direction framework element
                FrameworkElement focusElement = (FrameworkElement)FocusManager.FindNextElement(focusDirection);

                //Update framework element layout
                focusElement.UpdateLayout();

                //Focus on framework element
                await FocusManager.TryFocusAsync(focusElement, FocusState.Keyboard);

                Debug.WriteLine("Focused on direction framework element: " + focusElement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed focusing on direction element: " + ex.Message);
            }
        }

        /// <summary>
        /// Focus on target framework element
        /// </summary>
        public static async Task FocusFrameworkElement(FrameworkElement focusElement)
        {
            try
            {
                //Check if focus element is null
                if (focusElement == null)
                {
                    await FocusFrameworkElement(FocusNavigationDirection.Next);
                    Debug.WriteLine("Focus element is null, focusing on next element.");
                    return;
                }

                //Update framework element layout
                focusElement.UpdateLayout();

                //Focus on framework element
                await FocusManager.TryFocusAsync(focusElement, FocusState.Keyboard);

                Debug.WriteLine("Focused on target framework element: " + focusElement);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed focusing on target element: " + ex.Message);
            }
        }
    }
}