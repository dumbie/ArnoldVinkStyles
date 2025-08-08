using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using Windows.UI.Xaml.Controls;
using static ArnoldVinkStyles.AVFocus;

namespace ArnoldVinkStyles
{
    public partial class AVSortObservableCollection
    {
        public static void SortObservableCollection<T>(ListView listView, SortFunction<T> orderBy, Func<T, bool> where)
        {
            try
            {
                ObservableCollection<T> listObservable = (ObservableCollection<T>)listView.ItemsSource;
                SortObservableCollection(listView, listObservable, [orderBy], where);
            }
            catch { }
        }

        public static void SortObservableCollection<T>(ListView listView, List<SortFunction<T>> orderBy, Func<T, bool> where)
        {
            try
            {
                ObservableCollection<T> listObservable = (ObservableCollection<T>)listView.ItemsSource;
                SortObservableCollection(listView, listObservable, orderBy, where);
            }
            catch { }
        }

        public static void SortObservableCollection<T>(ListView listView, ObservableCollection<T> listObservable, SortFunction<T> orderBy, Func<T, bool> where)
        {
            try
            {
                SortObservableCollection(listView, listObservable, [orderBy], where);
            }
            catch { }
        }

        public static void SortObservableCollection<T>(ListView listView, ObservableCollection<T> listObservable, List<SortFunction<T>> orderBy, Func<T, bool> where)
        {
            try
            {
                Debug.WriteLine("Sorting ObservableCollection ListView");

                //Get current selected item
                dynamic selectedItem = listView.SelectedItem;

                //Filter list
                IEnumerable<T> whereEnumerable = null;
                if (where != null)
                {
                    whereEnumerable = listObservable.Where(where);
                }
                else
                {
                    whereEnumerable = listObservable;
                }

                //Sort list
                IOrderedEnumerable<T> sortEnumerable = null;
                foreach (SortFunction<T> orderFunc in orderBy)
                {
                    if (sortEnumerable == null)
                    {
                        if (orderFunc.Direction == SortDirection.Ascending || orderFunc.Direction == SortDirection.Default)
                        {
                            sortEnumerable = whereEnumerable.OrderBy(orderFunc.Function);
                        }
                        else
                        {
                            sortEnumerable = whereEnumerable.OrderByDescending(orderFunc.Function);
                        }
                    }
                    else
                    {
                        if (orderFunc.Direction == SortDirection.Ascending || orderFunc.Direction == SortDirection.Default)
                        {
                            sortEnumerable = sortEnumerable.ThenBy(orderFunc.Function);
                        }
                        else
                        {
                            sortEnumerable = sortEnumerable.ThenByDescending(orderFunc.Function);
                        }
                    }
                }

                //Move items
                int moveIndex = 0;
                foreach (T moveItem in sortEnumerable)
                {
                    listObservable.Move(listObservable.IndexOf(moveItem), moveIndex);
                    moveIndex++;
                }

                //Select focused item
                ListViewSelectItem(listView, selectedItem);
            }
            catch { }
        }
    }
}