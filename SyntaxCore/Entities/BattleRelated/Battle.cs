using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated;

[Table("Battles")]
public class Battle
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BattleId { get; set; }
    [ForeignKey("PlayerFK1")]
    [Required]
    public int PlayerId1 { get; set; }
    [ForeignKey("PlayerFK2")]
    [Required]
    public int PlayerId2 { get; set; }

    [ForeignKey("PlayerWinnerFK")]
    public int? PlayerWinnerId { get; set; }

    [MaxLength(255)]
    public string Category { get; set; }

    public DateTime StartedAt { get; set; }
    public DateTime EndedAt { get; set; }

    [MaxLength(50)]
    public string Status { get; set; }

    public User Player1 { get; set; }
    public User Player2 { get; set; }
    public User PlayerWinner { get; set; }

}