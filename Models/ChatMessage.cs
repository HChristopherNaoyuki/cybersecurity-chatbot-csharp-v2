// Models/ChatMessage.cs
namespace cybersecurity_chatbot_csharp_v2.Models
{
    /// <summary>
    /// Represents a chat message with sender information and content
    /// </summary>
    public class ChatMessage
    {
        /// <summary>
        /// Gets or sets the sender of the message (either "User" or "ChatBot")
        /// </summary>
        public string Sender { get; set; }

        /// <summary>
        /// Gets or sets the content of the message
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the message is from the user
        /// </summary>
        public bool IsUserMessage { get; set; }

        /// <summary>
        /// Initializes a new instance of the ChatMessage class
        /// </summary>
        public ChatMessage(string sender, string content, bool isUserMessage)
        {
            Sender = sender;
            Content = content;
            IsUserMessage = isUserMessage;
        }
    }
}