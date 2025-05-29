// Services/TaskService.cs
using cybersecurity_chatbot_csharp_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles task management functionality for cybersecurity tasks
    /// 
    /// Responsibilities:
    /// - Manages a collection of cybersecurity tasks
    /// - Handles task creation, completion, and reminders
    /// - Provides notifications for upcoming due tasks
    /// - Persists tasks between sessions
    /// </summary>
    public class TaskService
    {
        private readonly List<TaskItem> _tasks;
        private readonly FileService _fileService;
        private const string TasksFileName = "user_tasks.json";

        /// <summary>
        /// Event triggered when tasks are updated
        /// </summary>
        public event EventHandler TasksUpdated;

        /// <summary>
        /// Initializes a new instance of the TaskService class
        /// </summary>
        public TaskService(FileService fileService)
        {
            _fileService = fileService;
            _tasks = new List<TaskItem>();
            LoadTasks();
        }

        /// <summary>
        /// Adds a new cybersecurity task
        /// </summary>
        /// <param name="title">Task title</param>
        /// <param name="description">Task description</param>
        /// <param name="dueDate">Optional due date</param>
        /// <returns>The created task</returns>
        public TaskItem AddTask(string title, string description, DateTime? dueDate = null)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Task title cannot be empty");

            var task = new TaskItem(title, description, dueDate);
            _tasks.Add(task);
            SaveTasks();
            OnTasksUpdated();
            return task;
        }

        /// <summary>
        /// Marks a task as completed
        /// </summary>
        /// <param name="taskId">Creation date timestamp used as unique ID</param>
        public void CompleteTask(DateTime taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.CreatedDate == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                SaveTasks();
                OnTasksUpdated();
            }
        }

        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="taskId">Creation date timestamp used as unique ID</param>
        public void DeleteTask(DateTime taskId)
        {
            var task = _tasks.FirstOrDefault(t => t.CreatedDate == taskId);
            if (task != null)
            {
                _tasks.Remove(task);
                SaveTasks();
                OnTasksUpdated();
            }
        }

        /// <summary>
        /// Gets all incomplete tasks
        /// </summary>
        /// <returns>List of active tasks</returns>
        public List<TaskItem> GetActiveTasks()
        {
            return _tasks.Where(t => !t.IsCompleted).ToList();
        }

        /// <summary>
        /// Gets all tasks (both complete and incomplete)
        /// </summary>
        /// <returns>List of all tasks</returns>
        public List<TaskItem> GetAllTasks()
        {
            return _tasks.ToList();
        }

        /// <summary>
        /// Gets tasks that are due soon (within 2 days)
        /// </summary>
        /// <returns>List of upcoming due tasks</returns>
        public List<TaskItem> GetUpcomingTasks()
        {
            return _tasks
                .Where(t => !t.IsCompleted &&
                           t.DueDate.HasValue &&
                           t.DueDate.Value.Date >= DateTime.Today &&
                           t.DueDate.Value.Date <= DateTime.Today.AddDays(2))
                .ToList();
        }

        /// <summary>
        /// Saves tasks to persistent storage
        /// </summary>
        private void SaveTasks()
        {
            try
            {
                _fileService.SaveTasks(_tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving tasks: {ex.Message}");
            }
        }

        /// <summary>
        /// Loads tasks from persistent storage
        /// </summary>
        private void LoadTasks()
        {
            try
            {
                var loadedTasks = _fileService.LoadTasks();
                if (loadedTasks != null)
                {
                    _tasks.Clear();
                    _tasks.AddRange(loadedTasks);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading tasks: {ex.Message}");
            }
        }

        /// <summary>
        /// Raises the TasksUpdated event
        /// </summary>
        protected virtual void OnTasksUpdated()
        {
            TasksUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}