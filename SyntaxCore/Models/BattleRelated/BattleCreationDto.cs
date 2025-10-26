namespace SyntaxCore.Models.BattleRelated
{
    public record BattleCreationDto
    {
        public required string BattleName { get; set; } = string.Empty;
        public required int QuestionCount { get; set; }
        public required int TimePerQuestion { get; set; }
    }
}