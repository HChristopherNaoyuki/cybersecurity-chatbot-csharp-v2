// Utilities/FeedbackColorConverter.cs
using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace cybersecurity_chatbot_csharp_v2.Utilities
{
    /// <summary>
    /// Converts quiz answer correctness to color
    /// 
    /// True -> Green (correct)
    /// False -> Red (incorrect)
    /// </summary>
    public class FeedbackColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (value is bool b && b) ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}