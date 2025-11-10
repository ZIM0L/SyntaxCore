namespace SyntaxCore.Models.BattleRelated
{
    public class QuestionForBattleDto
    {
        public string QuestionText { get; set; } = string.Empty;
        public Dictionary<string, bool> AllAnswers { get; set; } = new Dictionary<string, bool>();
        public int TimeForAnswerInSeconds { get; set; }
    }
}
