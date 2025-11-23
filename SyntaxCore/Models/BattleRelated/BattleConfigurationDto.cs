namespace SyntaxCore.Models.BattleRelated
{
    public record BattleConfigurationDto
    {
        public required string Category { get; set; } = string.Empty;
        public required int Difficulty { get; set; }
        public required int QuestionCount { get; set; }
        public int? TimePerQuestionInSeconds { get; set; }
    }
}
