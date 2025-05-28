using cybersecurity_chatbot_csharp_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles the core chat functionality and conversation logic
    /// </summary>
    public class ChatService
    {
        private readonly Dictionary<string, string[]> _knowledgeBase;
        private readonly Dictionary<string, List<string>> _randomResponses;
        private readonly List<string> _ignoreWords;
        private readonly Random _random;
        private UserProfile _currentUser;

        /// <summary>
        /// Event triggered when a new chat message is generated
        /// </summary>
        public event EventHandler<ChatMessage> NewChatMessage;

        /// <summary>
        /// Initializes a new instance of the ChatService class
        /// </summary>
        public ChatService()
        {
            _random = new Random();
            _knowledgeBase = new Dictionary<string, string[]>();
            _randomResponses = new Dictionary<string, List<string>>();
            _ignoreWords = new List<string>();

            InitializeKnowledgeBase();
            InitializeRandomResponses();
            InitializeIgnoreWords();
        }

        /// <summary>
        /// Sets the current user profile
        /// </summary>
        public void SetUserProfile(UserProfile userProfile)
        {
            _currentUser = userProfile;
        }

        /// <summary>
        /// Processes user input and generates appropriate responses
        /// </summary>
        public void ProcessInput(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                RaiseNewMessage("ChatBot", "Please enter your question.", false);
                return;
            }

            // Check for special commands
            if (IsExitCommand(input))
            {
                RaiseNewMessage("ChatBot", "Stay safe online! Goodbye.", false);
                return;
            }

            if (IsHelpCommand(input))
            {
                DisplayHelp();
                return;
            }

            if (IsNameQuery(input))
            {
                HandleNameQuery();
                return;
            }

            if (IsHistoryCommand(input))
            {
                DisplayKeywordHistory();
                return;
            }

            if (IsFrequentQuestionQuery(input))
            {
                HandleFrequentQuestionQuery();
                return;
            }

            ProcessNaturalLanguage(input);
        }

        private void RaiseNewMessage(string sender, string content, bool isUserMessage)
        {
            NewChatMessage?.Invoke(this, new ChatMessage(sender, content, isUserMessage));
        }

        private bool IsExitCommand(string input)
        {
            string[] exitCommands = { "exit", "quit", "bye" };
            return exitCommands.Contains(input, StringComparer.OrdinalIgnoreCase);
        }

        private bool IsHelpCommand(string input)
        {
            string[] helpCommands = { "help", "options", "topics" };
            return helpCommands.Contains(input, StringComparer.OrdinalIgnoreCase);
        }

        private bool IsNameQuery(string input)
        {
            return input.IndexOf("what is my name", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("who am i", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private bool IsHistoryCommand(string input)
        {
            return input.Equals("history", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsFrequentQuestionQuery(string input)
        {
            return input.IndexOf("most frequently asked", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("faq", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("most common question", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("what do i ask most", StringComparison.OrdinalIgnoreCase) >= 0 ||
                   input.IndexOf("frequent questions", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private void DisplayHelp()
        {
            string message = "I can help with these cybersecurity topics:\n" +
                             string.Join("\n", _knowledgeBase.Keys.Where(k => k.Length > 3));
            RaiseNewMessage("ChatBot", message, false);
        }

        private void HandleNameQuery()
        {
            if (_currentUser == null)
            {
                RaiseNewMessage("ChatBot", "I don't know your name yet. What is your name?", false);
                return;
            }

            RaiseNewMessage("ChatBot", $"Your name is {_currentUser.Username}. Have you forgotten?", false);
        }

        private void DisplayKeywordHistory()
        {
            if (_currentUser == null || _currentUser.KeywordCounts.Count == 0)
            {
                RaiseNewMessage("ChatBot", "No keyword history available.", false);
                return;
            }

            string history = "Your keyword history:\n" +
                string.Join("\n", _currentUser.KeywordCounts
                    .OrderByDescending(kv => kv.Value)
                    .Select(kv => $"{kv.Key}: {kv.Value}"));

            RaiseNewMessage("ChatBot", history, false);
        }

        private void HandleFrequentQuestionQuery()
        {
            if (_currentUser == null)
            {
                RaiseNewMessage("ChatBot", "You haven't asked enough questions yet.", false);
                return;
            }

            string keyword = _currentUser.GetMostFrequentKeyword();
            if (keyword == null)
            {
                RaiseNewMessage("ChatBot", "You haven't asked enough questions yet.", false);
                return;
            }

            RaiseNewMessage("ChatBot", $"Your most frequently asked question is about '{keyword}'.", false);
        }

        private void ProcessNaturalLanguage(string input)
        {
            string sentiment = DetectSentiment(input);
            List<string> keywords = ExtractKeywords(input);

            // Save keywords to user profile
            if (_currentUser != null)
            {
                foreach (string keyword in keywords)
                {
                    _currentUser.RecordKeyword(keyword);
                }
            }

            if (TryHandleInterestExpression(input, keywords))
            {
                return;
            }

            DisplayMultiTopicResponses(keywords, sentiment);
        }

        private string DetectSentiment(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "neutral";

            var sentimentMap = new Dictionary<string, string[]>
            {
                ["worried"] = new[] { "worried", "concerned", "scared" },
                ["positive"] = new[] { "happy", "excited", "great" },
                ["negative"] = new[] { "angry", "frustrated", "upset" },
                ["curious"] = new[] { "what", "how", "explain", "?", "why" }
            };

            string lowerInput = input.ToLower();
            foreach (var sentiment in sentimentMap)
            {
                if (sentiment.Value.Any(keyword => lowerInput.Contains(keyword)))
                {
                    return sentiment.Key;
                }
            }
            return "neutral";
        }

        private List<string> ExtractKeywords(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new List<string>();

            return input.Split(new[] { ' ', ',', '.', '?', '!' },
                      StringSplitOptions.RemoveEmptyEntries)
                  .Select(word => word.ToLower().Trim())
                  .Where(word => word.Length > 2 && !_ignoreWords.Contains(word))
                  .ToList();
        }

        private bool TryHandleInterestExpression(string input, List<string> keywords)
        {
            if (!input.ToLower().Contains("interested in")) return false;

            foreach (string topic in _knowledgeBase.Keys)
            {
                if (input.ToLower().Contains(topic.ToLower()))
                {
                    if (_currentUser != null)
                    {
                        _currentUser.RecordKeyword(topic);
                    }

                    string response = GetResponse(topic);
                    RaiseNewMessage("ChatBot", response, false);
                    return true;
                }
            }
            return false;
        }

        private void DisplayMultiTopicResponses(List<string> keywords, string sentiment)
        {
            bool anyResponses = false;

            foreach (string keyword in keywords.Distinct())
            {
                int count = _currentUser?.GetKeywordCount(keyword) ?? 0;
                string response = GetContextualResponse(keyword, sentiment, count);

                if (!string.IsNullOrEmpty(response))
                {
                    anyResponses = true;
                    RaiseNewMessage("ChatBot", response, false);
                }
            }

            if (!anyResponses)
            {
                RaiseNewMessage("ChatBot", "I'm not sure about that. Try 'help' for options.", false);
            }
        }

        private string GetResponse(string topic)
        {
            if (string.IsNullOrEmpty(topic)) return null;

            if (_knowledgeBase.TryGetValue(topic, out string[] entry))
            {
                if (topic.Equals("password", StringComparison.OrdinalIgnoreCase))
                    return GetRandomPasswordResponse();
                if (topic.Equals("phishing", StringComparison.OrdinalIgnoreCase))
                    return GetRandomPhishingResponse();
                if (topic.Equals("privacy", StringComparison.OrdinalIgnoreCase))
                    return GetRandomPrivacyResponse();
                if (topic.Equals("vpn", StringComparison.OrdinalIgnoreCase))
                    return GetRandomVPNResponse();
                if (topic.Equals("wifi", StringComparison.OrdinalIgnoreCase))
                    return GetRandomWifiResponse();
                if (topic.Equals("email", StringComparison.OrdinalIgnoreCase))
                    return GetRandomEmailResponse();
                if (topic.Equals("2fa", StringComparison.OrdinalIgnoreCase))
                    return GetRandom2FAResponse();

                return entry[1];
            }
            return null;
        }

        private string GetContextualResponse(string keyword, string sentiment, int count)
        {
            string response = GetResponse(keyword);
            if (string.IsNullOrEmpty(response)) return null;

            // Add contextual prefix if discussed before
            if (count > 1)
            {
                var contextualPrefixes = new Dictionary<int, string[]>
                {
                    [2] = new[]
                    {
                        $"Since we discussed {keyword} before, ",
                        $"About {keyword} again, ",
                        $"Regarding {keyword}, "
                    },
                    [3] = new[]
                    {
                        $"As we've talked about {keyword} several times, ",
                        $"You seem interested in {keyword}, ",
                        $"Since you keep asking about {keyword}, "
                    },
                    [4] = new[]
                    {
                        $"You're really curious about {keyword}, ",
                        $"I notice you frequently ask about {keyword}, ",
                        $"You've asked about {keyword} {count} times now, "
                    }
                };

                // Find the closest matching tier
                int tier = contextualPrefixes.Keys
                    .Where(k => k <= count)
                    .DefaultIfEmpty(0)
                    .Max();

                if (tier > 0 && contextualPrefixes.TryGetValue(tier, out var prefixes))
                {
                    response = prefixes[_random.Next(prefixes.Length)] + response;
                }
            }

            // Add sentiment prefix if needed
            if (sentiment != "neutral")
            {
                response = $"{GetSentimentResponse(sentiment)}{response}";
            }

            return $"{keyword.ToUpper()}: {response}";
        }

        private string GetSentimentResponse(string sentiment)
        {
            switch (sentiment)
            {
                case "worried": return "I understand this can be concerning. ";
                case "positive": return "Great! I'm glad you're enthusiastic! ";
                case "negative": return "I'm sorry you're feeling frustrated. ";
                case "curious": return "That's a great question! ";
                default: return "";
            }
        }

        private string GetRandomPasswordResponse()
        {
            return GetRandomResponse("password");
        }

        private string GetRandomPhishingResponse()
        {
            return GetRandomResponse("phishing");
        }

        private string GetRandomPrivacyResponse()
        {
            return GetRandomResponse("privacy");
        }

        private string GetRandomVPNResponse()
        {
            return GetRandomResponse("vpn");
        }

        private string GetRandomWifiResponse()
        {
            return GetRandomResponse("wifi");
        }

        private string GetRandomEmailResponse()
        {
            return GetRandomResponse("email");
        }

        private string GetRandom2FAResponse()
        {
            return GetRandomResponse("2fa");
        }

        private string GetRandomResponse(string topic)
        {
            if (_randomResponses.TryGetValue(topic, out var responses) && responses.Count > 0)
            {
                return responses[_random.Next(responses.Count)];
            }
            return "I don't have information about that topic.";
        }

        private void InitializeKnowledgeBase()
        {
            _knowledgeBase["how are you"] = new[] { "greeting", "I'm functioning optimally! Ready to discuss cybersecurity." };
            _knowledgeBase["purpose"] = new[] { "purpose", "I provide cybersecurity education to help you stay safe online." };
            _knowledgeBase["help"] = new[] { "help", "I can explain: Passwords, 2FA, phishing, VPNs, Wi-Fi security, email safety" };

            _knowledgeBase["password"] = new[] { "password", GetRandomPasswordResponse() };
            _knowledgeBase["2fa"] = new[] { "2fa", GetRandom2FAResponse() };
            _knowledgeBase["phishing"] = new[] { "phishing", GetRandomPhishingResponse() };
            _knowledgeBase["vpn"] = new[] { "vpn", GetRandomVPNResponse() };
            _knowledgeBase["wifi"] = new[] { "wifi", GetRandomWifiResponse() };
            _knowledgeBase["email"] = new[] { "email", GetRandomEmailResponse() };
            _knowledgeBase["privacy"] = new[] { "privacy", GetRandomPrivacyResponse() };
        }

        private void InitializeRandomResponses()
        {
            // Password responses
            _randomResponses["password"] = new List<string>
            {
                "Strong passwords should be at least 12 characters long and include a mix of uppercase, lowercase, numbers and symbols.",
                "Consider using a passphrase instead of a password - something like 'PurpleElephant$JumpedOver42Clouds!'",
                "Never reuse passwords across different accounts. Use a password manager to keep track of them all securely.",
                "Change your passwords immediately if a service you use reports a data breach.",
                "The strongest passwords are long, random, and unique to each account."
            };

            // 2FA responses
            _randomResponses["2fa"] = new List<string>
            {
                "Two-factor authentication adds security by requiring:\n1. Something you know (password)\n2. Something you have (phone/device)\nUse authenticator apps instead of SMS when possible",
                "2FA protects you even if your password is compromised. Always enable it for important accounts!",
                "Authenticator apps like Google Authenticator or Authy provide more secure 2FA than SMS codes.",
                "Security keys (like YubiKey) provide the strongest form of two-factor authentication."
            };

            // Phishing responses
            _randomResponses["phishing"] = new List<string>
            {
                "Phishing emails often create a sense of urgency. Always verify unusual requests through another channel.",
                "Check the sender's email address carefully - phishing attempts often use addresses that look similar to legitimate ones.",
                "Hover over links before clicking to see the actual URL. If it looks suspicious, don't click!",
                "Legitimate organizations will never ask for your password or sensitive information via email."
            };

            // Privacy responses
            _randomResponses["privacy"] = new List<string>
            {
                "Review privacy settings on all your social media accounts regularly - they often change their policies.",
                "Be careful what personal information you share online - it can be used for social engineering attacks.",
                "Consider using privacy-focused browsers and search engines that don't track your activity.",
                "Use private browsing mode when accessing sensitive accounts on shared computers."
            };

            // VPN responses
            _randomResponses["vpn"] = new List<string>
            {
                "VPN benefits:\n- Encrypts all internet traffic\n- Essential on public Wi-Fi\n- Choose no-log providers\n- Doesn't provide complete anonymity",
                "A good VPN should have:\n- Strong encryption\n- No-logs policy\n- Fast servers\n- Reliable connection",
                "While VPNs enhance privacy, they don't make you completely anonymous online."
            };

            // WiFi responses
            _randomResponses["wifi"] = new List<string>
            {
                "Public Wi-Fi usage tips:\n- Avoid sensitive activities\n- Use a VPN\n- Disable file sharing\n- Turn off auto-connect",
                "Home WiFi security tips:\n- Use WPA3 encryption\n- Change default admin password\n- Disable WPS\n- Create guest network",
                "Never conduct banking or shopping on public WiFi without a VPN."
            };

            // Email responses
            _randomResponses["email"] = new List<string>
            {
                "Email safety tips:\n- Enable spam filters\n- Verify unusual requests\n- Don't open unexpected attachments",
                "Watch for these email red flags:\n- Urgent requests for action\n- Poor grammar/spelling\n- Suspicious attachments\n- Requests for credentials",
                "Use separate email accounts for important services like banking versus casual signups."
            };
        }

        private void InitializeIgnoreWords()
        {
            string[] wordsToIgnore = new string[]
            {
                "tell", "me", "about", "a", "the", "an",
                "do", "explain", "can",
                "what", "is", "does", "could", "would",
                "should", "will", "please", "thanks", "thank"
            };

            foreach (string word in wordsToIgnore)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    _ignoreWords.Add(word.ToLower());
                }
            }
        }
    }
}