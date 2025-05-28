// Services/MemoryService.cs
using cybersecurity_chatbot_csharp_v2.Models;
using System.Collections.Generic;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Manages user profiles and memory functionality
    /// </summary>
    public class MemoryService
    {
        private readonly FileService _fileService;
        private readonly Dictionary<string, UserProfile> _userProfiles;
        private UserProfile _currentUser;

        /// <summary>
        /// Initializes a new instance of the MemoryService class
        /// </summary>
        public MemoryService(FileService fileService)
        {
            _fileService = fileService;
            _userProfiles = _fileService.LoadUserProfiles();
        }

        /// <summary>
        /// Gets or sets the current user profile
        /// </summary>
        public UserProfile CurrentUser
        {
            get => _currentUser;
            set => _currentUser = value;
        }

        /// <summary>
        /// Creates a new user profile or retrieves an existing one
        /// </summary>
        public UserProfile GetOrCreateUser(string username)
        {
            if (string.IsNullOrWhiteSpace(username)) return null;

            string normalizedUsername = username.ToLower().Trim();

            if (_userProfiles.TryGetValue(normalizedUsername, out UserProfile profile))
            {
                _currentUser = profile;
                return profile;
            }

            var newUser = new UserProfile(username);
            _userProfiles[normalizedUsername] = newUser;
            _currentUser = newUser;
            _fileService.SaveUserProfiles(_userProfiles);
            return newUser;
        }

        /// <summary>
        /// Saves all user profiles to file
        /// </summary>
        public void SaveProfiles()
        {
            _fileService.SaveUserProfiles(_userProfiles);
        }
    }
}