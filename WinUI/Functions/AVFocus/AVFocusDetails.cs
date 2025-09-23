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
        //Focus on AVFocusDetails
        public static async Task AVFocusDetailsFocus(AVFocusDetails focusElement)
        {
            try
            {
                //Focus on AVFocusDetails
                if (focusElement.FocusElement != null)
                {
                    Debug.WriteLine("Focusing on previous element: " + focusElement.FocusElement);
                    await FocusFrameworkElement(focusElement.FocusElement);
                }
                else if (focusElement.FocusListView != null)
                {
                    Debug.WriteLine("Focusing on previous listbox: " + focusElement.FocusListView);
                    await ListViewFocusIndex(focusElement.FocusListView, false, focusElement.FocusIndex);
                }
                else
                {
                    await FocusFrameworkElement(FocusNavigationDirection.Next);
                    Debug.WriteLine("No previous focus element, focusing on next element.");
                }

                //Reset AVFocusDetails
                focusElement.Reset();
            }
            catch { }
        }

        //Save AVFocusDetails
        public static void AVFocusDetailsSave(AVFocusDetails saveElement, FrameworkElement focusedElement)
        {
            try
            {
                //Get the currently focused element
                if (focusedElement == null)
                {
                    focusedElement = GetFocusedFrameworkElement();
                }

                //Check the currently focused element
                if (focusedElement != null)
                {
                    //Validate focus type
                    if (focusedElement is Page)
                    {
                        Debug.WriteLine("Invalid element focus type: " + focusedElement.GetType());
                        saveElement = null;
                        return;
                    }

                    //Check if save element is null
                    if (saveElement == null)
                    {
                        Debug.WriteLine("Save element is null creating new.");
                        saveElement = new AVFocusDetails();
                    }

                    //Save focused element
                    saveElement.FocusElement = focusedElement;

                    //Save listbox details
                    if (focusedElement is ListViewItem)
                    {
                        saveElement.FocusListView = AVVisualTree.FindVisualParent<ListView>(saveElement.FocusElement);
                        saveElement.FocusIndex = saveElement.FocusListView.SelectedIndex;
                    }
                    else if (focusedElement is ListView)
                    {
                        saveElement.FocusListView = (ListView)saveElement.FocusElement;
                        saveElement.FocusIndex = saveElement.FocusListView.SelectedIndex;
                    }

                    Debug.WriteLine("Saved element focus: " + focusedElement + " / index: " + saveElement.FocusIndex);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed saving element focus: " + ex.Message);
            }
        }
    }
}