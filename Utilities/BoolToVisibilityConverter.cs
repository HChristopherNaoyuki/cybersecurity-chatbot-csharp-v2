// Utilities/BoolToVisibilityConverter.cs
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace cybersecurity_chatbot_csharp_v2.Utilities
{
    /// <summary>
    /// Converts boolean values to Visibility enum values
    /// 
    /// Usage:
    /// True -> Visibility.Visible
    /// False -> Visibility.Collapsed
    /// 
    /// Implements IValueConverter for WPF binding
    /// </summary>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolToVisibilityConverter : IValueConverter
    {
        /// <summary>
        /// Converts boolean to Visibility
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Visibility.Visible : Visibility.Collapsed;
        }

        /// <summary>
        /// Converts Visibility back to boolean (not typically used)
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Visibility v && v == Visibility.Visible;
        }
    }
}