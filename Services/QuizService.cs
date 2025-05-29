using cybersecurity_chatbot_csharp_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Manages the cybersecurity quiz functionality including:
    /// - Question repository
    /// - Quiz state management
    /// - Scoring and progress tracking
    /// - Answer validation and feedback
    /// </summary>
    public class QuizService
    {
        private readonly List<QuizQuestion> _questions;
        private int _currentQuestionIndex;
        private int _score;
        private bool _quizActive;

        /// <summary>
        /// Event arguments for quiz state changes
        /// Contains all relevant quiz progress information
        /// </summary>
        public class QuizStateChangedEventArgs : EventArgs
        {
            public required QuizQuestion CurrentQuestion { get; set; }
            public int SelectedAnswerIndex { get; set; } = -1;
            public bool IsCorrect { get; set; }
            public int Score { get; set; }
            public bool QuizActive { get; set; }
            public bool QuizComplete { get; set; }
            public bool ShowFeedback { get; set; }
            public int QuestionNumber { get; set; }
            public int TotalQuestions { get; set; }
        }

        /// <summary>
        /// Event triggered when quiz state changes
        /// (question advances, answer submitted, quiz completes)
        /// </summary>
        public event EventHandler<QuizStateChangedEventArgs> QuizStateChanged;

        /// <summary>
        /// Initializes quiz service and loads questions
        /// </summary>
        public QuizService()
        {
            _questions = new List<QuizQuestion>();
            InitializeQuestions();
        }

        /// <summary>
        /// Starts a new quiz session
        /// Resets all progress tracking
        /// </summary>
        public void StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = true;

            // Notify subscribers of new quiz state
            RaiseQuizStateChanged(new QuizStateChangedEventArgs
            {
                CurrentQuestion = _questions[_currentQuestionIndex],
                Score = _score,
                QuizActive = _quizActive,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count
            });
        }

        /// <summary>
        /// Submits an answer for the current question
        /// </summary>
        /// <param name="selectedIndex">Index of selected answer (0-based)</param>
        public void SubmitAnswer(int selectedIndex)
        {
            if (!_quizActive) return;

            var currentQuestion = _questions[_currentQuestionIndex];
            bool isCorrect = selectedIndex == currentQuestion.CorrectAnswerIndex;

            // Update score if correct
            if (isCorrect)
            {
                _score++;
            }

            // Notify subscribers of answer result
            RaiseQuizStateChanged(new QuizStateChangedEventArgs
            {
                CurrentQuestion = currentQuestion,
                SelectedAnswerIndex = selectedIndex,
                IsCorrect = isCorrect,
                Score = _score,
                QuizActive = _quizActive,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count,
                ShowFeedback = true
            });
        }

        /// <summary>
        /// Advances to the next question or ends quiz if complete
        /// </summary>
        public void NextQuestion()
        {
            if (!_quizActive) return;

            _currentQuestionIndex++;

            // Check if quiz is complete
            if (_currentQuestionIndex >= _questions.Count)
            {
                _quizActive = false;
                RaiseQuizStateChanged(new QuizStateChangedEventArgs
                {
                    QuizComplete = true,
                    Score = _score,
                    QuizActive = _quizActive,
                    TotalQuestions = _questions.Count
                });
                return;
            }

            // Notify subscribers of new question
            RaiseQuizStateChanged(new QuizStateChangedEventArgs
            {
                CurrentQuestion = _questions[_currentQuestionIndex],
                Score = _score,
                QuizActive = _quizActive,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count
            });
        }

        /// <summary>
        /// Initializes the quiz questions repository
        /// </summary>
        private void InitializeQuestions()
        {
            // Question 1
            _questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new[]
                {
                    "Reply with your password",
                    "Delete the email",
                    "Report the email as phishing",
                    "Ignore it"
                },
                2, // Correct answer index (0-based)
                "You should report phishing attempts to help protect others."
            ));

            // Question 2
            _questions.Add(new QuizQuestion(
                "Which of these is the strongest password?",
                new[]
                {
                    "password123",
                    "P@ssw0rd",
                    "CorrectHorseBatteryStaple",
                    "12345678"
                },
                2,
                "Long passphrases are stronger than complex but short passwords."
            ));

            // Add more questions following the same pattern...
            // Typically would have 10 questions as per requirements
        }

        /// <summary>
        /// Helper method to raise the QuizStateChanged event
        /// </summary>
        private void RaiseQuizStateChanged(QuizStateChangedEventArgs args)
        {
            QuizStateChanged?.Invoke(this, args);
        }
    }
}