using SyntaxCore.Entities.UserRelated;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.BattleRelated;

[Table("QuestionFlags")]
[Comment("Stores flags raised by users on questions, including reasons and review status.")]
public class QuestionFlag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid FlagId { get; set; } = Guid.NewGuid();
    public Guid QuestionFK { get; set; }
    public Guid UserFK { get; set; }
    public Guid? UserReviewerFK { get; set; }
    [MaxLength(255)]
    public string Reason { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ReviewedAt { get; set; }
    public bool IsResolved { get; set; }

    // Navigation
    [ForeignKey(nameof(QuestionFK))]
    public Question Question { get; set; } = null!;
    [ForeignKey(nameof(UserFK))]
    [InverseProperty(nameof(User.QuestionFlags))]
    public User User { get; set; } = null!;
    [ForeignKey(nameof(UserReviewerFK))]
    [InverseProperty(nameof(User.ReviewedFlags))]

    public User Reviewer { get; set; } = null!;
}