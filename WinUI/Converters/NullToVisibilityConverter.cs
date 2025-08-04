using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace ArnoldVinkStyles
{
    //Note:
    //Consider using TargetNullValue='Collapsed'
    //Import:
    //xmlns:AVStyles="using:ArnoldVinkStyles"
    //Usage:
    //Visibility="{Binding Name, Converter={AVStyles:NullToVisibilityConverter}}"/>
    public partial class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        protected override object ProvideValue()
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return Visibility.Collapsed;
            }

            Type valueType = value.GetType();
            if (valueType == typeof(string))
            {
                return string.IsNullOrWhiteSpace((string)value) ? Visibility.Collapsed : Visibility.Visible;
            }
            else if (valueType == typeof(bool))
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}