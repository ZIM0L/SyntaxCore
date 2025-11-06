namespace SyntaxCore.Models.BattleRelated
{
    public class QuestionForBattleDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public List<string> AllAnswers { get; set; } = new List<string>();
        public int TimeForAnswerInSeconds { get; set; }
    }
}
