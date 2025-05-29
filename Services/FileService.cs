// Services/FileService.cs
using cybersecurity_chatbot_csharp_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles file operations for user profiles and data persistence
    /// </summary>
    public class FileService
    {
        private readonly string _dataDirectory;
        private readonly string _userProfilesFile;

        public object JsonConvert { get; private set; }

        /// <summary>
        /// Initializes a new instance of the FileService class
        /// </summary>
        public FileService()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _userProfilesFile = Path.Combine(_dataDirectory, "user_profiles.json");

            Directory.CreateDirectory(_dataDirectory);
        }

        /// <summary>
        /// Saves user profiles to file
        /// </summary>
        public void SaveUserProfiles(Dictionary<string, UserProfile> userProfiles)
        {
            try
            {
                string json = JsonConvert.SerializeObject(userProfiles, Formatting.Indented);
                File.WriteAllText(_userProfilesFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving user profiles: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads user profiles from file
        /// </summary>
        public Dictionary<string, UserProfile> LoadUserProfiles()
        {
            try
            {
                if (!File.Exists(_userProfilesFile))
                {
                    return new Dictionary<string, UserProfile>();
                }

                string json = File.ReadAllText(_userProfilesFile);
                return JsonConvert.DeserializeObject<Dictionary<string, UserProfile>>(json) ??
                       new Dictionary<string, UserProfile>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading user profiles: {ex.Message}");
                return new Dictionary<string, UserProfile>();
            }
        }

        internal void SaveTasks(List<TaskItem> tasks)
        {
            throw new NotImplementedException();
        }
    }
}