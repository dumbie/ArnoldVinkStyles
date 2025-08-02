using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using static ArnoldVinkStyles.AVVisualTree;

namespace ArnoldVinkStyles
{
    public class AVListView
    {
        /// <summary>
        /// Get index of ListViewItem object
        /// </summary>
        public static int GetListViewItemObjectIndex(ListView targetListView, object targetObject)
        {
            try
            {
                return targetListView.Items.IndexOf(targetObject);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get ListViewItem index: " + ex.Message);
                return -1;
            }
        }

        /// <summary>
        /// Get routed ListViewItem object
        /// </summary>
        public static T GetRoutedListViewItemObject<T>(RoutedEventArgs e) where T : class
        {
            try
            {
                ListViewItem routedContainer = GetRoutedListViewItemContainer(e);
                return GetListViewItemObjectFromContainer<T>(routedContainer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get routed ListViewItem object: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get routed ListViewItem container
        /// </summary>
        public static ListViewItem GetRoutedListViewItemContainer(RoutedEventArgs e)
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
        /// Get focused ListViewItem object
        /// </summary>
        public static T GetFocusedListViewItemObject<T>(ListView targetListView) where T : class
        {
            try
            {
                ListViewItem focusedContainer = GetFocusedListViewItemContainer(targetListView);
                return GetListViewItemObjectFromContainer<T>(focusedContainer);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get focused ListViewItem object: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get focused ListViewItem container
        /// </summary>
        public static ListViewItem GetFocusedListViewItemContainer(ListView targetListView)
        {
            try
            {
                //Update layout
                targetListView.UpdateLayout();

                //Get all ListViewItem containers
                List<ListViewItem> listViewItems = FindVisualChildren<ListViewItem>(targetListView);

                //Search for focused container
                return listViewItems.Where(x => x.FocusState != FocusState.Unfocused).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get focused ListViewItem container: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get ListViewItem container from ListViewItem object
        /// Note: ListView must be visible to successfully extract ListViewItem
        /// </summary>
        public static ListViewItem GetListViewItemContainerFromObject(ListView targetListView, object targetObject)
        {
            try
            {
                //Update layout
                targetListView.UpdateLayout();

                //Get all ListViewItem containers
                List<ListViewItem> listViewItems = FindVisualChildren<ListViewItem>(targetListView);

                //Search for target object
                return listViewItems.Where(x => x.Content == targetObject).FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get ListViewItem container: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get ListViewItem object from ListViewItem container
        /// </summary>
        public static T GetListViewItemObjectFromContainer<T>(ListViewItem targetListViewItem) where T : class
        {
            try
            {
                return (T)targetListViewItem.Content;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed to get ListViewItem object: " + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Get ScrollViewer from ListView
        /// </summary>
        public static ScrollViewer GetListViewScrollViewer(ListView targetListView)
        {
            try
            {
                //Check visibility
                bool visibilityChanged = false;
                Visibility visibilityOriginal = targetListView.Visibility;
                if (visibilityOriginal != Visibility.Visible)
                {
                    visibilityChanged = true;
                    targetListView.Visibility = Visibility.Visible;
                }

                //Update layout
                targetListView.UpdateLayout();

                //Get ScrollViewer
                ScrollViewer scrollViewer = FindVisualChild<ScrollViewer>(targetListView);

                //Check visibility
                if (visibilityChanged)
                {
                    targetListView.Visibility = visibilityOriginal;
                }

                //Return result
                return scrollViewer;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Failed getting ScrollViewer from ListView: " + ex.Message);
                return null;
            }
        }
    }
}