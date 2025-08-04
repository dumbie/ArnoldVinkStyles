using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="using:ArnoldVinkStyles"
    //Usage:
    //Visibility="{Binding Enum, ConverterParameter='EnumAsString', Converter={AVStyles:MatchToVisibilityConverter}}"/>
    public partial class MatchToVisibilityConverter : MarkupExtension, IValueConverter
    {
        protected override object ProvideValue()
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value.ToString() == parameter.ToString()) ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return null;
        }
    }
}