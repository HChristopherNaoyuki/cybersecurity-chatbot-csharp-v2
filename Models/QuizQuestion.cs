// Models/QuizQuestion.cs
namespace cybersecurity_chatbot_csharp_v2.Models
{
    /// <summary>
    /// Represents a quiz question with possible answers and the correct answer
    /// </summary>
    public class QuizQuestion
    {
        /// <summary>
        /// Gets or sets the question text
        /// </summary>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the list of possible answers
        /// </summary>
        public string[] Answers { get; set; }

        /// <summary>
        /// Gets or sets the index of the correct answer
        /// </summary>
        public int CorrectAnswerIndex { get; set; }

        /// <summary>
        /// Gets or sets the explanation for the correct answer
        /// </summary>
        public string Explanation { get; set; }

        /// <summary>
        /// Initializes a new instance of the QuizQuestion class
        /// </summary>
        public QuizQuestion(string question, string[] answers, int correctAnswerIndex, string explanation)
        {
            Question = question;
            Answers = answers;
            CorrectAnswerIndex = correctAnswerIndex;
            Explanation = explanation;
        }
    }
}