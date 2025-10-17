using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.BattleRelated;

[Table("QuestionFlags")]
public class QuestionFlag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int FlagId { get; set; }

    [ForeignKey("QuestionFK")]
    public int QuestionId { get; set; }

    [ForeignKey("UserFK")]
    public int UserId { get; set; }

    [MaxLength(255)]
    public string Reason { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }

    [ForeignKey(nameof(Reviewer))]
    public int? ReviewerId { get; set; }

    public bool IsResolved { get; set; }

    public Question Question { get; set; }
    public User User { get; set; }
    public User Reviewer { get; set; }
}