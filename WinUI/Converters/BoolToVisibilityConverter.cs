using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="using:ArnoldVinkStyles"
    //Usage:
    //Visibility="{Binding Name, Converter={AVStyles:BoolToVisibilityConverter}}"/>
    public partial class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        protected override object ProvideValue()
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}