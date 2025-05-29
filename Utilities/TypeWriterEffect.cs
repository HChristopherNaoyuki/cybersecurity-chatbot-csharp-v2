// Utilities/TypeWriterEffect.cs
using System;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Media;

namespace cybersecurity_chatbot_csharp_v2.Utilities
{
    /// <summary>
    /// Simulates a typewriter effect for text display
    /// 
    /// Features:
    /// - Animates text appearance character by character
    /// - Supports customizable delay between characters
    /// - Works with WPF FlowDocument for rich text support
    /// </summary>
    public static class TypeWriterEffect
    {
        /// <summary>
        /// Displays text with a typewriter effect in a FlowDocument
        /// </summary>
        /// <param name="document">FlowDocument to display text in</param>
        /// <param name="text">Text to display</param>
        /// <param name="delayMs">Delay between characters in milliseconds</param>
        /// <param name="color">Text color</param>
        public static void TypeText(FlowDocument document, string text, int delayMs = 30, Color? color = null)
        {
            if (string.IsNullOrEmpty(text)) return;

            var paragraph = new Paragraph();
            document.Blocks.Add(paragraph);

            foreach (char c in text)
            {
                var run = new Run(c.ToString());
                if (color.HasValue)
                {
                    run.Foreground = new SolidColorBrush(color.Value);
                }
                paragraph.Inlines.Add(run);
                Thread.Sleep(delayMs);
            }
        }
    }
}