using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated
{

    [Table("BattlesConfiguration")]
    [Comment("Configuration settings for battles, including question count, category, and difficulty.")]
    public class BattleConfiguration
    {
        [Key]
        public Guid BattleConfigurationId { get; set; } = Guid.NewGuid();
        public Guid BattleFK { get; set; }
        [Required]
        public int QuestionCount { get; set; }
        [MaxLength(255)]
        public string Category { get; set; } = string.Empty;
        public int Difficulty { get; set; }

        // Navigation
        [ForeignKey(nameof(BattleFK))]
        public Battle Battle { get; set; } = null!;
    }
}