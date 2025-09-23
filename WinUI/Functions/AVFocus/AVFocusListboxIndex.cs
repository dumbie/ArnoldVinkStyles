using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using static ArnoldVinkStyles.AVListView;

namespace ArnoldVinkStyles
{
    public partial class AVFocus
    {
        //Focus on listbox index
        public static async Task ListViewFocusIndex(ListView focusListView, bool lastIndex, int indexNumber)
        {
            try
            {
                //Check if listbox is null
                if (focusListView == null)
                {
                    await FocusFrameworkElement(FocusNavigationDirection.Next);
                    Debug.WriteLine("Listbox cannot be focused on, focusing on next element.");
                    return;
                }

                //Check if listbox has items
                if (focusListView.Items.Count == 0)
                {
                    await FocusFrameworkElement(FocusNavigationDirection.Next);
                    Debug.WriteLine("Listbox has no items, focusing on next element.");
                    return;
                }

                //Update listbox layout
                focusListView.UpdateLayout();

                //Select a listbox index
                ListViewSelectIndex(focusListView, lastIndex, indexNumber);

                //Get target listview item
                object scrollListViewItem = focusListView.SelectedItem;

                //Scroll to listbox item
                focusListView.ScrollIntoView(scrollListViewItem);

                //Force focus on element
                ListViewItem focusListViewItem = GetListViewItemContainerFromObject(focusListView, scrollListViewItem);
                await FocusFrameworkElement(focusListViewItem);

                Debug.WriteLine("Focused on listbox index: " + scrollListViewItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed focusing on listbox index: " + ex.Message);
            }
        }

        //Select listbox index
        public static void ListViewSelectIndex(ListView focusListView, bool lastIndex, int indexNumber)
        {
            try
            {
                //Update the listbox layout
                focusListView.UpdateLayout();

                //Get listbox item count
                int itemsCount = focusListView.Items.Count;

                //Select the listbox index
                if (lastIndex)
                {
                    focusListView.SelectedIndex = itemsCount - 1;
                    Debug.WriteLine("Selecting last listbox index.");
                }
                else if (indexNumber > -1)
                {
                    if (indexNumber >= itemsCount)
                    {
                        focusListView.SelectedIndex = itemsCount - 1;
                        Debug.WriteLine("Selecting last listbox index.");
                    }
                    else
                    {
                        focusListView.SelectedIndex = indexNumber;
                        Debug.WriteLine("Selecting listbox index: " + indexNumber);
                    }
                }

                //Check the selected index
                if (focusListView.SelectedIndex <= -1)
                {
                    focusListView.SelectedIndex = 0;
                    Debug.WriteLine("No selection, selecting first listbox index.");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed selecting the listbox index: " + ex.Message);
            }
        }
    }
}