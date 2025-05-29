// ViewModels/QuizViewModel.cs
using cybersecurity_chatbot_csharp_v2.Models;
using cybersecurity_chatbot_csharp_v2.Services;
using System.ComponentModel;
using System.Windows.Input;

namespace cybersecurity_chatbot_csharp_v2.ViewModels
{
    /// <summary>
    /// ViewModel for the quiz functionality
    /// 
    /// Responsibilities:
    /// - Manages the quiz state and progress
    /// - Handles user answers and scoring
    /// - Provides feedback for each question
    /// </summary>
    public class QuizViewModel : INotifyPropertyChanged
    {
        private readonly QuizService _quizService;
        private QuizQuestion _currentQuestion;
        private int _score;
        private int _questionNumber;
        private int _totalQuestions;
        private bool _showFeedback;
        private bool _isCorrect;
        private bool _quizComplete;

        /// <summary>
        /// Event for property change notifications
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Current quiz question
        /// </summary>
        public QuizQuestion CurrentQuestion
        {
            get => _currentQuestion;
            set
            {
                _currentQuestion = value;
                OnPropertyChanged(nameof(CurrentQuestion));
            }
        }

        /// <summary>
        /// Current quiz score
        /// </summary>
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                OnPropertyChanged(nameof(Score));
            }
        }

        /// <summary>
        /// Current question number
        /// </summary>
        public int QuestionNumber
        {
            get => _questionNumber;
            set
            {
                _questionNumber = value;
                OnPropertyChanged(nameof(QuestionNumber));
            }
        }

        /// <summary>
        /// Total number of questions
        /// </summary>
        public int TotalQuestions
        {
            get => _totalQuestions;
            set
            {
                _totalQuestions = value;
                OnPropertyChanged(nameof(TotalQuestions));
            }
        }

        /// <summary>
        /// Indicates whether to show feedback for the last answer
        /// </summary>
        public bool ShowFeedback
        {
            get => _showFeedback;
            set
            {
                _showFeedback = value;
                OnPropertyChanged(nameof(ShowFeedback));
            }
        }

        /// <summary>
        /// Indicates whether the last answer was correct
        /// </summary>
        public bool IsCorrect
        {
            get => _isCorrect;
            set
            {
                _isCorrect = value;
                OnPropertyChanged(nameof(IsCorrect));
            }
        }

        /// <summary>
        /// Indicates whether the quiz is complete
        /// </summary>
        public bool QuizComplete
        {
            get => _quizComplete;
            set
            {
                _quizComplete = value;
                OnPropertyChanged(nameof(QuizComplete));
            }
        }

        /// <summary>
        /// Command for submitting an answer
        /// </summary>
        public ICommand SubmitAnswerCommand { get; }

        /// <summary>
        /// Command for moving to the next question
        /// </summary>
        public ICommand NextQuestionCommand { get; }

        /// <summary>
        /// Initializes a new instance of the QuizViewModel class
        /// </summary>
        public QuizViewModel(QuizService quizService)
        {
            _quizService = quizService;
            _quizService.QuizStateChanged += OnQuizStateChanged;

            SubmitAnswerCommand = new RelayCommand<int>(SubmitAnswer);
            NextQuestionCommand = new RelayCommand(NextQuestion);
        }

        /// <summary>
        /// Starts a new quiz
        /// </summary>
        public void StartQuiz()
        {
            _quizService.StartQuiz();
        }

        /// <summary>
        /// Submits an answer to the current question
        /// </summary>
        private void SubmitAnswer(int answerIndex)
        {
            _quizService.SubmitAnswer(answerIndex);
        }

        /// <summary>
        /// Moves to the next question
        /// </summary>
        private void NextQuestion()
        {
            _quizService.NextQuestion();
        }

        /// <summary>
        /// Handles quiz state changes
        /// </summary>
        private void OnQuizStateChanged(object sender, QuizService.QuizStateChangedEventArgs e)
        {
            CurrentQuestion = e.CurrentQuestion;
            Score = e.Score;
            QuestionNumber = e.QuestionNumber;
            TotalQuestions = e.TotalQuestions;
            ShowFeedback = e.ShowFeedback;
            IsCorrect = e.IsCorrect;
            QuizComplete = e.QuizComplete;
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