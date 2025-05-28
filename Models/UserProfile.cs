using System.Collections.Generic;

namespace cybersecurity_chatbot_csharp_v2.Models
{
    /// <summary>
    /// Represents a user profile with keyword history
    /// </summary>
    public class UserProfile
    {
        /// <summary>
        /// Gets or sets the username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the dictionary of keywords and their counts
        /// </summary>
        public Dictionary<string, int> KeywordCounts { get; set; }

        /// <summary>
        /// Initializes a new instance of the UserProfile class
        /// </summary>
        public UserProfile(string username)
        {
            Username = username;
            KeywordCounts = new Dictionary<string, int>();
        }

        /// <summary>
        /// Records a keyword in the user's history
        /// </summary>
        public void RecordKeyword(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            string normalized = keyword.ToLower().Trim();

            if (KeywordCounts.ContainsKey(normalized))
            {
                KeywordCounts[normalized]++;
            }
            else
            {
                KeywordCounts[normalized] = 1;
            }
        }

        /// <summary>
        /// Gets the count for a specific keyword
        /// </summary>
        public int GetKeywordCount(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return 0;
            string normalized = keyword.ToLower().Trim();
            return KeywordCounts.TryGetValue(normalized, out int count) ? count : 0;
        }

        /// <summary>
        /// Gets the most frequently asked keyword
        /// </summary>
        public string GetMostFrequentKeyword()
        {
            if (KeywordCounts.Count == 0) return null;

            string maxKey = null;
            int maxValue = 0;

            foreach (var pair in KeywordCounts)
            {
                if (pair.Value > maxValue)
                {
                    maxKey = pair.Key;
                    maxValue = pair.Value;
                }
            }

            return maxKey;
        }
    }
}