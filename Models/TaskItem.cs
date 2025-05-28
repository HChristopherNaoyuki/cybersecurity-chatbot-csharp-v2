// Models/TaskItem.cs
using System;

namespace cybersecurity_chatbot_csharp_v2.Models
{
    /// <summary>
    /// Represents a cybersecurity task with optional reminder
    /// </summary>
    public class TaskItem
    {
        /// <summary>
        /// Gets or sets the title of the task
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description of the task
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the task is completed
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// Gets or sets the due date for the task (optional)
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date of the task
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Initializes a new instance of the TaskItem class
        /// </summary>
        public TaskItem(string title, string description, DateTime? dueDate = null)
        {
            Title = title;
            Description = description;
            DueDate = dueDate;
            IsCompleted = false;
            CreatedDate = DateTime.Now;
        }
    }
}