// Utilities/AsciiArtConverter.cs
using System;
using System.Drawing;
using System.IO;
using System.Text;

namespace cybersecurity_chatbot_csharp_v2.Utilities
{
    /// <summary>
    /// Converts images to ASCII art for display in the console
    /// 
    /// Algorithm:
    /// 1. Resize the image to desired width/height while maintaining aspect ratio
    /// 2. Convert each pixel to grayscale
    /// 3. Map grayscale values to ASCII characters of varying density
    /// 4. Build the ASCII string representation
    /// </summary>
    public static class AsciiArtConverter
    {
        private static readonly char[] AsciiChars = { '#', '8', '&', 'o', ':', '*', '.', ' ' };

        /// <summary>
        /// Converts an image file to ASCII art
        /// </summary>
        /// <param name="imagePath">Path to the image file</param>
        /// <param name="width">Width of ASCII output (in characters)</param>
        /// <param name="height">Height of ASCII output (in characters)</param>
        /// <returns>ASCII art string</returns>
        public static string ConvertImageToAscii(string imagePath, int width, int height)
        {
            try
            {
                using (Bitmap bitmap = new Bitmap(imagePath))
                {
                    Bitmap resizedImage = new Bitmap(bitmap, new Size(width, height));
                    StringBuilder asciiArt = new StringBuilder();

                    for (int y = 0; y < resizedImage.Height; y++)
                    {
                        for (int x = 0; x < resizedImage.Width; x++)
                        {
                            Color pixelColor = resizedImage.GetPixel(x, y);
                            int grayValue = (int)(0.3 * pixelColor.R + 0.59 * pixelColor.G + 0.11 * pixelColor.B);
                            char asciiChar = MapGrayValueToAscii(grayValue);
                            asciiArt.Append(asciiChar);
                        }
                        asciiArt.AppendLine();
                    }

                    return asciiArt.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error converting image to ASCII: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Maps a grayscale value (0-255) to an ASCII character
        /// </summary>
        private static char MapGrayValueToAscii(int grayValue)
        {
            grayValue = Math.Min(Math.Max(grayValue, 0), 255);
            int index = grayValue * (AsciiChars.Length - 1) / 255;
            return AsciiChars[index];
        }
    }
}