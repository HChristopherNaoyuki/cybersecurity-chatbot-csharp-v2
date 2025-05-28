// Services/QuizService.cs
using cybersecurity_chatbot_csharp_v2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace cybersecurity_chatbot_csharp_v2.Services
{
    /// <summary>
    /// Handles the cybersecurity quiz functionality
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
        /// Initializes a new instance of the QuizService class
        /// </summary>
        public QuizService()
        {
            _questions = new List<QuizQuestion>();
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
            OnQuizStateChanged(new QuizStateChangedEventArgs
            {
                CurrentQuestion = _questions[_currentQuestionIndex],
                Score = _score,
                QuizActive = _quizActive,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count
            });
        }

        /// <summary>
        /// Submits an answer to the current question
        /// </summary>
        public void SubmitAnswer(int selectedIndex)
        {
            if (!_quizActive) return;

            var currentQuestion = _questions[_currentQuestionIndex];
            bool isCorrect = selectedIndex == currentQuestion.CorrectAnswerIndex;

            if (isCorrect)
            {
                _score++;
            }

            OnQuizStateChanged(new QuizStateChangedEventArgs
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
        /// Moves to the next question in the quiz
        /// </summary>
        public void NextQuestion()
        {
            if (!_quizActive) return;

            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _questions.Count)
            {
                _quizActive = false;
                OnQuizStateChanged(new QuizStateChangedEventArgs
                {
                    QuizComplete = true,
                    Score = _score,
                    QuizActive = _quizActive,
                    TotalQuestions = _questions.Count
                });
                return;
            }

            OnQuizStateChanged(new QuizStateChangedEventArgs
            {
                CurrentQuestion = _questions[_currentQuestionIndex],
                Score = _score,
                QuizActive = _quizActive,
                QuestionNumber = _currentQuestionIndex + 1,
                TotalQuestions = _questions.Count
            });
        }

        private void InitializeQuestions()
        {
            _questions.Add(new QuizQuestion(
                "What should you do if you receive an email asking for your password?",
                new[] { "Reply with your password", "Delete the email", "Report the email as phishing", "Ignore it" },
                2,
                "You should report phishing attempts to help protect others."
            ));

            _questions.Add(new QuizQuestion(
                "Which of these is the strongest password?",
                new[] { "password123", "P@ssw0rd", "CorrectHorseBatteryStaple", "12345678" },
                2,
                "Long passphrases are stronger than complex but short passwords."
            ));

            _questions.Add(new QuizQuestion(
                "When should you use a VPN?",
                new[] { "Only when traveling", "Only for banking", "On all public Wi-Fi networks", "Never" },
                2,
                "VPNs encrypt your traffic on insecure networks like public Wi-Fi."
            ));

            _questions.Add(new QuizQuestion(
                "What is two-factor authentication (2FA)?",
                new[] { "Using two passwords", "A backup email address", "A second verification method after password", "A type of firewall" },
                2,
                "2FA requires something you know (password) and something you have (phone/device)."
            ));

            _questions.Add(new QuizQuestion(
                "How often should you update your software?",
                new[] { "Never", "Only when it stops working", "When updates are available", "Once a year" },
                2,
                "Software updates often include critical security patches."
            ));

            _questions.Add(new QuizQuestion(
                "What is a common sign of a phishing website?",
                new[] { "HTTPS in the URL", "Slightly misspelled domain name", "Professional design", "Contact information" },
                1,
                "Scammers often use domains like 'g00gle.com' to trick users."
            ));

            _questions.Add(new QuizQuestion(
                "How can you check if a website connection is secure?",
                new[] { "Look for HTTPS and padlock icon", "Check the website design", "See if it loads quickly", "Ask a friend" },
                0,
                "HTTPS encrypts data between your browser and the website."
            ));

            _questions.Add(new QuizQuestion(
                "What should you do before clicking a link in an email?",
                new[] { "Click immediately if it looks interesting", "Hover to see the actual URL", "Forward to friends first", "Check your horoscope" },
                1,
                "Always verify links by hovering before clicking."
            ));

            _questions.Add(new QuizQuestion(
                "Why shouldn't you use the same password everywhere?",
                new[] { "It's hard to remember", "If one account is compromised, all are at risk", "Websites don't allow it", "It slows down your computer" },
                1,
                "Password reuse creates a single point of failure."
            ));

            _questions.Add(new QuizQuestion(
                "What's the best way to handle sensitive documents?",
                new[] { "Email them to yourself", "Store in cloud storage with 2FA", "Keep only paper copies", "Memorize them" },
                1,
                "Secure cloud storage with 2FA is safer than email or paper."
            ));
        }

        protected virtual void OnQuizStateChanged(QuizStateChangedEventArgs e)
        {
            QuizStateChanged?.Invoke(this, e);
        }
    }

    /// <summary>
    /// Event arguments for quiz state changes
    /// </summary>
    public class QuizStateChangedEventArgs : EventArgs
    {
        public QuizQuestion CurrentQuestion { get; set; }
        public int SelectedAnswerIndex { get; set; } = -1;
        public bool IsCorrect { get; set; }
        public int Score { get; set; }
        public bool QuizActive { get; set; }
        public bool QuizComplete { get; set; }
        public bool ShowFeedback { get; set; }
        public int QuestionNumber { get; set; }
        public int TotalQuestions { get; set; }
    }
}