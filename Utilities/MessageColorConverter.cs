// Utilities/MessageColorConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace cybersecurity_chatbot_csharp_v2.Utilities
{
    /// <summary>
    /// Converts message origin to color
    /// 
    /// True (user message) -> Blue
    /// False (bot message) -> Purple
    /// </summary>
    public class MessageColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Brushes.Blue : Brushes.Purple;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}