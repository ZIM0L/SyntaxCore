using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Constants;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Battles")]
[Comment("Records of battles between players, including participants, outcomes, and timestamps.")]
public class Battle
{
    public Battle(string battleName)
    {
        BattleName = battleName;
    }

    [Key]
    public Guid BattleId { get; set; } = Guid.NewGuid();
    public Guid BattlePublicId { get; set; } = Guid.NewGuid();
    public string BattleName { get; set; } = string.Empty;
    public Guid BattleOwnerFK { get; set; } 
    public Guid? PlayerWinnerFK { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }

    [MaxLength(50)]
    public string Status { get; set; } = BattleStatuses.Waiting;
    public int maxPlayers { get; set; } = 2;

    // Navigation
    [ForeignKey(nameof(PlayerWinnerFK))]
    [InverseProperty(nameof(User.BattlesWon))]
    public User? PlayerWinner { get; set; } = null;

    [ForeignKey(nameof(BattleOwnerFK))]
    [InverseProperty(nameof(User.BattlesOwn))]
    public User BattleOwner { get; set; } = null!;

    public ICollection<BattleParticipant> BattleParticipants { get; set; } = new List<BattleParticipant>();
    public ICollection<BattleConfiguration> BattleConfigurations { get; set; } = new List<BattleConfiguration>();
}