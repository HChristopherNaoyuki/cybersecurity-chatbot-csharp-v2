// Services/FileService.cs
using cybersecurity_chatbot_csharp_v2.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles file operations for user profiles and data persistence
    /// 
    /// Responsibilities:
    /// - Manages serialization/deserialization of user profiles
    /// - Handles task persistence
    /// - Ensures data directory exists
    /// - Provides error handling for file operations
    /// </summary>
    public class FileService
    {
        private readonly string _dataDirectory;
        private readonly string _userProfilesFile;
        private readonly string _tasksFile;

        /// <summary>
        /// Initializes a new instance of the FileService class
        /// </summary>
        public FileService()
        {
            _dataDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            _userProfilesFile = Path.Combine(_dataDirectory, "user_profiles.json");
            _tasksFile = Path.Combine(_dataDirectory, "tasks.json");

            Directory.CreateDirectory(_dataDirectory);
        }

        /// <summary>
        /// Saves user profiles to file
        /// </summary>
        /// <param name="userProfiles">Dictionary of user profiles to save</param>
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
        /// <returns>Dictionary of loaded user profiles</returns>
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

        /// <summary>
        /// Saves tasks to file
        /// </summary>
        /// <param name="tasks">List of tasks to save</param>
        public void SaveTasks(List<TaskItem> tasks)
        {
            try
            {
                string json = JsonConvert.SerializeObject(tasks, Formatting.Indented);
                File.WriteAllText(_tasksFile, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads tasks from file
        /// </summary>
        /// <returns>List of loaded tasks</returns>
        public List<TaskItem> LoadTasks()
        {
            try
            {
                if (!File.Exists(_tasksFile))
                {
                    return new List<TaskItem>();
                }

                string json = File.ReadAllText(_tasksFile);
                return JsonConvert.DeserializeObject<List<TaskItem>>(json) ??
                       new List<TaskItem>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
                return new List<TaskItem>();
            }
        }
    }
}