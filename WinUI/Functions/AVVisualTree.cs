using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace ArnoldVinkStyles
{
    public partial class AVVisualTree
    {
        //Find all visual children of object
        public static List<T> FindVisualChildren<T>(DependencyObject objChild) where T : DependencyObject
        {
            List<T> childrenList = new List<T>();
            try
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(objChild); i++)
                {
                    try
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(objChild, i);
                        if (child != null)
                        {
                            if (child is T)
                            {
                                childrenList.Add((T)child);
                            }
                            else
                            {
                                foreach (T childOfChild in FindVisualChildren<T>(child))
                                {
                                    childrenList.Add(childOfChild);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            return childrenList;
        }

        //Find the visual child of object
        public static T FindVisualChild<T>(DependencyObject objChild) where T : DependencyObject
        {
            try
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(objChild); i++)
                {
                    try
                    {
                        DependencyObject child = VisualTreeHelper.GetChild(objChild, i);
                        if (child != null)
                        {
                            if (child is T)
                            {
                                return (T)child;
                            }
                            else
                            {
                                T subChild = FindVisualChild<T>(child);
                                if (subChild != null) { return subChild; }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch { }
            return null;
        }

        //Find the visual parent of object
        public static T FindVisualParent<T>(DependencyObject objChild) where T : DependencyObject
        {
            try
            {
                DependencyObject parentObject = VisualTreeHelper.GetParent(objChild);
                if (parentObject == null) { return null; }

                //Check if parent matches the type
                T parent = parentObject as T;
                if (parent != null)
                {
                    return parent;
                }
                else
                {
                    return FindVisualParent<T>(parentObject);
                }
            }
            catch { }
            return null;
        }
    }
}