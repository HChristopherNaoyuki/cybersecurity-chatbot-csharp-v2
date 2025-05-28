using System;
using System.IO;
using System.Media;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles audio playback for the chatbot
    /// </summary>
    public class AudioService
    {
        private readonly string _audioDirectory;

        /// <summary>
        /// Initializes a new instance of the AudioService class
        /// </summary>
        public AudioService()
        {
            _audioDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audio");
            Directory.CreateDirectory(_audioDirectory);
        }

        /// <summary>
        /// Plays the welcome audio greeting
        /// </summary>
        public void PlayWelcomeGreeting()
        {
            try
            {
                string audioPath = Path.Combine(_audioDirectory, "welcome.wav");

                if (File.Exists(audioPath))
                {
                    using (SoundPlayer player = new SoundPlayer(audioPath))
                    {
                        player.Load();  // Pre-load for smooth playback
                        player.PlaySync();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error playing audio: {ex.Message}");
            }
        }
    }
}