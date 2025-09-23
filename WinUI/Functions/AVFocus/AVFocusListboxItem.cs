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
        //Focus on listbox item
        public static async Task ListViewFocusItem(ListView focusListView, object selectItem)
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

                //Select a listbox item
                ListViewSelectItem(focusListView, selectItem);

                //Get target listview item
                object scrollListViewItem = focusListView.SelectedItem;

                //Scroll to listbox item
                focusListView.ScrollIntoView(scrollListViewItem);

                //Force focus on element
                ListViewItem focusListViewItem = GetListViewItemContainerFromObject(focusListView, scrollListViewItem);
                await FocusFrameworkElement(focusListViewItem);

                Debug.WriteLine("Focused on listbox item: " + scrollListViewItem);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed focusing on listbox item: " + ex.Message);
            }
        }

        //Select listbox item
        public static void ListViewSelectItem(ListView focusListView, object selectItem)
        {
            try
            {
                //Update the listbox layout
                focusListView.UpdateLayout();

                //Select the listbox item
                if (selectItem != null)
                {
                    try
                    {
                        focusListView.SelectedItem = selectItem;
                        Debug.WriteLine("Selecting listbox item: " + selectItem);
                    }
                    catch { }
                }
                else
                {
                    Debug.WriteLine("Select listbox item is null.");
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
                Debug.WriteLine("Failed selecting the listbox item: " + ex.Message);
            }
        }
    }
}