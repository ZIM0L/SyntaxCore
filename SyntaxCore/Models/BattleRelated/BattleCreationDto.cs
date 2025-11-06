namespace SyntaxCore.Models.BattleRelated
{
    public record BattleCreationDto
    {
        public required string BattleName { get; set; } = string.Empty;
        public int MaxPlayers { get; set; } = 2;
        public required int TimePerQuestion { get; set; }
        public required List<BattleConfigurationDto> Configurations { get; set; } = new();
    }
}