using SyntaxCore.Constants;

namespace SyntaxCore.Models.BattleRelated
{
    public class BattleDto
    {
        public Guid BattleId { get; set; }        
        public string BattleName { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int QuestionsCount { get; set; }
        public string PlayerId1 { get; set; } = string.Empty;
        public string? PlayerId2 { get; set; } = string.Empty; 
        public string Status { get; set; } = BattleStatuses.Waiting;
        public DateTime CreatedAt { get; set; }    
        public DateTime? EndedAt { get; set; }      
    }

}