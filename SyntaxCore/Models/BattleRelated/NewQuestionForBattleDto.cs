namespace SyntaxCore.Controllers
{
    public class NewQuestionForBattleDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public List<string> CorrectAnswers { get; set; } = new List<string>();
        public List<string> WrongAnswers { get; set; } = new List<string>();
        public int Difficulty { get; set; }
        public string? Explanation { get; set; } = string.Empty;
        public int TimeForAnswerInSeconds { get; set; }
    }
}