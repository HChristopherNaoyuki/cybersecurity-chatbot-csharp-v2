using cybersecurity_chatbot_csharp_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles quiz functionality for the cybersecurity chatbot
    /// </summary>
    public class QuizService
    {
        private readonly List<QuizQuestion> _questions;
        private int _currentQuestionIndex;
        private int _score;
        private bool _quizActive;

        /// <summary>
        /// Event triggered when the quiz state changes
        /// </summary>
        public event EventHandler<QuizStateChangedEventArgs> QuizStateChanged;

        /// <summary>
        /// Gets the current question
        /// </summary>
        public QuizQuestion CurrentQuestion => _quizActive && _currentQuestionIndex < _questions.Count
            ? _questions[_currentQuestionIndex]
            : null;

        /// <summary>
        /// Gets the current score
        /// </summary>
        public int Score => _score;

        /// <summary>
        /// Gets the total number of questions
        /// </summary>
        public int TotalQuestions => _questions.Count;

        /// <summary>
        /// Gets a value indicating whether the quiz is active
        /// </summary>
        public bool IsQuizActive => _quizActive;

        /// <summary>
        /// Initializes a new instance of the QuizService class
        /// </summary>
        public QuizService()
        {
            _questions = new List<QuizQuestion>();
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = false;

            InitializeQuestions();
        }

        /// <summary>
        /// Starts a new quiz
        /// </summary>
        public void StartQuiz()
        {
            _currentQuestionIndex = 0;
            _score = 0;
            _quizActive = true;
            OnQuizStateChanged();
        }

        /// <summary>
        /// Ends the current quiz
        /// </summary>
        public void EndQuiz()
        {
            _quizActive = false;
            OnQuizStateChanged();
        }

        /// <summary>
        /// Submits an answer to the current question
        /// </summary>
        public bool SubmitAnswer(int answerIndex)
        {
            if (!_quizActive || CurrentQuestion == null) return false;

            bool isCorrect = answerIndex == CurrentQuestion.CorrectAnswerIndex;
            if (isCorrect)
            {
                _score++;
            }

            OnQuizStateChanged(isCorrect ? "Correct!" : "Incorrect!", CurrentQuestion.Explanation);

            _currentQuestionIndex++;
            if (_currentQuestionIndex >= _questions.Count)
            {
                EndQuiz();
            }
            else
            {
                OnQuizStateChanged();
            }

            return isCorrect;
        }

        /// <summary>
        /// Raises the QuizStateChanged event
        /// </summary>
        protected virtual void OnQuizStateChanged(string message = null, string explanation = null)
        {
            QuizStateChanged?.Invoke(this, new QuizStateChangedEventArgs
            {
                CurrentQuestion = CurrentQuestion,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count,
                Score = _score,
                QuizActive = _quizActive,
                Message = message,
                Explanation = explanation
            });
        }

        private void InitializeQuestions()
        {
            _questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "Reporting phishing emails helps prevent scams."
            ));

            _questions.Add(new QuizQuestion(
                "Which of these is the strongest password?",
                new[] { "password123", "P@ssw0rd", "CorrectHorseBatteryStaple", "12345678" },
                2,
                "Long passphrases are stronger than complex but short passwords."
            ));

            _questions.Add(new QuizQuestion(
                "When using public Wi-Fi, you should:",
                new[] { "Do online banking as usual", "Use a VPN for sensitive activities", "Disable your firewall", "Share files freely" },
                1,
                "VPNs encrypt your traffic on public networks."
            ));

            _questions.Add(new QuizQuestion(
                "Two-factor authentication typically requires:",
                new[] { "Two different passwords", "A password and a fingerprint", "A