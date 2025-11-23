using SyntaxCore.Constants;

namespace SyntaxCore.Models.BattleRelated
{
    public class BattleDto
    {
        public Guid BattlePublicId { get; set; }
        public string BattleName { get; set; } = string.Empty;
        public string BattleOwner { get; set; } = string.Empty;
        public string Status { get; set; } = BattleStatuses.Waiting;
        public int TotalQuestionsCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public int MaxPlayers { get; set; }
        public int CurrentPlayers { get; set; }
        public int QuestionsCount { get; set; }
        public List<BattleConfigurationDto> BattleConfiguration { get; set; } = new();
    }

}