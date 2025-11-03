namespace SyntaxCore.Models.BattleRelated
{
    public class QuestionForBattleDto
    {
        public required string QuestionText { get; set; }
        public required List<string> AllAnswers { get; set; }
        public required List<string> CorrectAnswers { get; set; }
        public required int TimeForAnswerInSeconds { get; set; }
    }
}
