﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ArnoldVinkStyles
{
    //Import:
    //xmlns:AVStyles="clr-namespace:ArnoldVinkStyles;assembly=ArnoldVinkStyles"
    //Usage:
    //Visibility="{Binding Name, Converter={AVStyles:NullToVisibilityConverter}}"/>

    public class NullToVisibilityConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
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

                if (valueType == typeof(bool))
                {
                    return (bool)value ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch { }
            return Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}