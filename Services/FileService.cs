using cybersecurity_chatbot_csharp_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting; // Explicitly specify Newtonsoft.Json.Formatting

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles all file operations for the application including:
    /// - User profile persistence
    /// - Task item storage
    /// - Data directory management
    /// 
    /// Uses JSON serialization for data storage
    /// Implements error handling for file operations
    /// </summary>
    public class FileService
    {
        private readonly string _dataDirectory;
        private readonly string _userProfilesFile;
        private readonly string _tasksFile;

        /// <summary>
        /// Initializes a new instance of FileService
        /// Creates required data directory if it doesn't exist
        /// </summary>
        public FileService()
        {
            // Set up file paths
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _userProfilesFile = Path.Combine(_dataDirectory, "user_profiles.json");
            _tasksFile = Path.Combine(_dataDirectory, "tasks.json");

            // Ensure data directory exists
            Directory.CreateDirectory(_dataDirectory);
        }

        /// <summary>
        /// Saves user profiles to JSON file
        /// </summary>
        /// <param name="userProfiles">Dictionary of username to UserProfile mappings</param>
        public void SaveUserProfiles(Dictionary<string, UserProfile> userProfiles)
        {
            try
            {
                // Serialize to JSON with indentation for readability
                string json = JsonConvert.SerializeObject(userProfiles, Formatting.Indented);

                // Write to file (creates or overwrites)
                File.WriteAllText(_userProfilesFile, json);
            }
            catch (Exception ex)
            {
                // Log error but don't crash the application
                Console.WriteLine($"[ERROR] Failed to save user profiles: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads user profiles from JSON file
        /// </summary>
        /// <returns>Dictionary of username to UserProfile mappings</returns>
        public Dictionary<string, UserProfile> LoadUserProfiles()
        {
            try
            {
                // Return empty dictionary if file doesn't exist
                if (!File.Exists(_userProfilesFile))
                {
                    return new Dictionary<string, UserProfile>();
                }

                // Read and deserialize JSON
                string json = File.ReadAllText(_userProfilesFile);
                return JsonConvert.DeserializeObject<Dictionary<string, UserProfile>>(json)
                    ?? new Dictionary<string, UserProfile>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load user profiles: {ex.Message}");
                return new Dictionary<string, UserProfile>();
            }
        }

        /// <summary>
        /// Saves task list to JSON file
        /// </summary>
        /// <param name="tasks">List of TaskItem objects to save</param>
        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                // Serialize with indentation
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(_tasksFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to save tasks: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads task list from JSON file
        /// </summary>
        /// <returns>List of TaskItem objects</returns>
        public List<TaskItem> LoadTasks()
        {
            try
            {
                // Return empty list if file doesn't exist
                if (!File.Exists(_tasksFile))
                {
                    return new List<TaskItem>();
                }

                // Read and deserialize JSON
                string json = File.ReadAllText(_tasksFile);
                return JsonConvert.DeserializeObject<List<TaskItem>>(json)
                    ?? new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to load tasks: {ex.Message}");
                return new List<TaskItem>();
            }
        }
    }
}