// ViewModels/MainViewModel.cs
using cybersecurity_chatbot_csharp_v2.Models;
using cybersecurity_chatbot_csharp_v2.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Media;

namespace cybersecurity_chatbot_csharp_v2.ViewModels
{
    /// <summary>
    /// Main view model for the application that coordinates all services and UI interactions
    /// 
    /// Responsibilities:
    /// - Manages user profiles and authentication
    /// - Coordinates between chat, quiz, and task services
    /// - Handles UI commands and updates
    /// - Maintains application state
    /// </summary>
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ChatService _chatService;
        private readonly QuizService _quizService;
        private readonly TaskService _taskService;
        private readonly MemoryService _memoryService;
        private readonly AudioService _audioService;

        private string _currentUsername;
        private string _userInput;
        private bool _isQuizActive;

        /// <summary>
        /// Event for property change notifications
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Collection of chat messages
        /// </summary>
        public ObservableCollection<ChatMessage> Messages { get; } = new ObservableCollection<ChatMessage>();

        /// <summary>
        /// Collection of cybersecurity tasks
        /// </summary>
        public ObservableCollection<TaskItem> Tasks { get; } = new ObservableCollection<TaskItem>();

        /// <summary>
        /// Current username
        /// </summary>
        public string CurrentUsername
        {
            get => _currentUsername;
            set
            {
                _currentUsername = value;
                OnPropertyChanged(nameof(CurrentUsername));
            }
        }

        /// <summary>
        /// Current user input
        /// </summary>
        public string UserInput
        {
            get => _userInput;
            set
            {
                _userInput = value;
                OnPropertyChanged(nameof(UserInput));
            }
        }

        /// <summary>
        /// Indicates if a quiz is currently active
        /// </summary>
        public bool IsQuizActive
        {
            get => _isQuizActive;
            set
            {
                _isQuizActive = value;
                OnPropertyChanged(nameof(IsQuizActive));
            }
        }

        /// <summary>
        /// Command for sending user input
        /// </summary>
        public ICommand SendCommand { get; }

        /// <summary>
        /// Command for starting a new quiz
        /// </summary>
        public ICommand StartQuizCommand { get; }

        /// <summary>
        /// Command for adding a new task
        /// </summary>
        public ICommand AddTaskCommand { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class
        /// </summary>
        public MainViewModel(
            ChatService chatService,
            QuizService quizService,
            TaskService taskService,
            MemoryService memoryService,
            AudioService audioService)
        {
            _chatService = chatService;
            _quizService = quizService;
            _taskService = taskService;
            _memoryService = memoryService;
            _audioService = audioService;

            // Initialize commands
            SendCommand = new RelayCommand(SendUserInput);
            StartQuizCommand = new RelayCommand(StartNewQuiz);
            AddTaskCommand = new RelayCommand(AddNewTask);

            // Subscribe to events
            _chatService.NewChatMessage += OnNewChatMessage;
            _quizService.QuizStateChanged += OnQuizStateChanged;
            _taskService.TasksUpdated += OnTasksUpdated;

            // Initialize with welcome message
            InitializeWelcome();
        }

        /// <summary>
        /// Initializes the welcome sequence
        /// </summary>
        private void InitializeWelcome()
        {
            // Play welcome audio
            _audioService.PlayWelcomeGreeting();

            // Add welcome message
            Messages.Add(new ChatMessage("ChatBot", "Welcome to the Cybersecurity Awareness Bot!", false));
            Messages.Add(new ChatMessage("ChatBot", "Please enter your name to begin:", false));
        }

        /// <summary>
        /// Handles sending user input
        /// </summary>
        private void SendUserInput()
        {
            if (string.IsNullOrWhiteSpace(UserInput)) return;

            // Add user message to chat
            Messages.Add(new ChatMessage(CurrentUsername ?? "User", UserInput, true));

            if (CurrentUsername == null)
            {
                // First message is assumed to be username
                CurrentUsername = UserInput.Trim();
                _memoryService.GetOrCreateUser(CurrentUsername);
                Messages.Add(new ChatMessage("ChatBot", $"Hello, {CurrentUsername}! How can I help you with cybersecurity today?", false));
            }
            else
            {
                if (IsQuizActive)
                {
                    // Handle quiz answers
                    if (int.TryParse(UserInput, out int answerIndex))
                    {
                        _quizService.SubmitAnswer(answerIndex - 1); // Convert to 0-based index
                    }
                }
                else
                {
                    // Handle normal chat input
                    _chatService.ProcessInput(UserInput);
                }
            }

            UserInput = string.Empty;
        }

        /// <summary>
        /// Starts a new cybersecurity quiz
        /// </summary>
        private void StartNewQuiz()
        {
            IsQuizActive = true;
            _quizService.StartQuiz();
        }

        /// <summary>
        /// Adds a new cybersecurity task
        /// </summary>
        private void AddNewTask()
        {
            // In a real implementation, this would open a dialog for task details
            // For now, we'll add a sample task
            var task = _taskService.AddTask(
                "Enable Two-Factor Authentication",
                "Add 2FA to your important accounts for better security",
                DateTime.Now.AddDays(7));

            Messages.Add(new ChatMessage("ChatBot", $"Added task: {task.Title} (Due: {task.DueDate?.ToShortDateString() ?? "No due date"})", false));
        }

        /// <summary>
        /// Handles new chat messages from services
        /// </summary>
        private void OnNewChatMessage(object sender, ChatMessage message)
        {
            Messages.Add(message);
        }

        /// <summary>
        /// Handles quiz state changes
        /// </summary>
        private void OnQuizStateChanged(object sender, QuizService.QuizStateChangedEventArgs e)
        {
            if (e.CurrentQuestion != null)
            {
                Messages.Add(new ChatMessage("ChatBot", $"Question {e.QuestionNumber}/{e.TotalQuestions}: {e.CurrentQuestion.Question}", false));
                for (int i = 0; i < e.CurrentQuestion.Answers.Length; i++)
                {
                    Messages.Add(new ChatMessage("ChatBot", $"{i + 1}. {e.CurrentQuestion.Answers[i]}", false));
                }
            }

            if (e.ShowFeedback)
            {
                string feedback = e.IsCorrect ? "Correct!" : "Incorrect!";
                Messages.Add(new ChatMessage("ChatBot", $"{feedback} {e.CurrentQuestion.Explanation}", false));
            }

            if (e.QuizComplete)
            {
                IsQuizActive = false;
                Messages.Add(new ChatMessage("ChatBot", $"Quiz complete! Your score: {e.Score}/{e.TotalQuestions}", false));
            }
        }

        /// <summary>
        /// Handles task updates
        /// </summary>
        private void OnTasksUpdated(object sender, EventArgs e)
        {
            Tasks.Clear();
            foreach (var task in _taskService.GetActiveTasks())
            {
                Tasks.Add(task);
            }
        }

        /// <summary>
        /// Raises the PropertyChanged event
        /// </summary>
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}