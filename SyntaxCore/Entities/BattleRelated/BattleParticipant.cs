using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using SyntaxCore.Entities.UserRelated;

namespace SyntaxCore.Entities.BattleRelated
{
    [Table("BattleParticipants")]
    [Comment("Links users to battles with roles and stats.")]
    public class BattleParticipant
    {
        [Key]
        public Guid BattleParticipantId { get; set; } = Guid.NewGuid();
        [Required]
        public Guid BattleFK { get; set; }
        [Required]
        public Guid UserFK { get; set; }
        [MaxLength(50)]
        public string Role { get; set; } = string.Empty;
        public int Score { get; set; } = 0;

        // Navigation
        [ForeignKey(nameof(UserFK))]
        public User User { get; set; } = null!;
        [ForeignKey(nameof(BattleFK))]
        public Battle Battle { get; set; } = null!;
    }
}
