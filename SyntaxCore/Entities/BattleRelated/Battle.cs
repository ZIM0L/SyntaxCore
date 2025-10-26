using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Battles")]
[Comment("Records of battles between players, including participants, outcomes, and timestamps.")]
public class Battle
{
    public Battle(string battleName, int questionsCount)
    {
        BattleName = battleName;
        QuestionsCount = questionsCount;
    }

    [Key]
    public Guid BattleId { get; set; } = Guid.NewGuid();
    public string BattleName { get; set; } = string.Empty;
    public Guid? PlayerWinnerFK { get; set; }

    [MaxLength(255)]
    public string Category { get; set; } = string.Empty;

    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = string.Empty;
    public int QuestionsCount { get; set; }

    // Navigation
    [ForeignKey(nameof(PlayerWinnerFK))]
    public User? PlayerWinner { get; set; }

    public ICollection<BattleParticipant> BattleParticipant { get; set; } = new List<BattleParticipant>();
}