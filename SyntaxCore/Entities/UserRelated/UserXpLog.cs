using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.UserRelated;

[Table("UserXpLog")]
public class UserXpLog
{
    [Key]
    public int XpLogId { get; set; }

    [ForeignKey(nameof(User))]
    public int UserId { get; set; }

    [MaxLength(255)]
    public string Source { get; set; }

    public int Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; }
}