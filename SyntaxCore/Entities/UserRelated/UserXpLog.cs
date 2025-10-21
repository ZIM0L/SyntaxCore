using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.UserRelated;

[Table("UserXpLog")]
[Comment("Logs of experience points (XP) changes for users, including sources and amounts.")]
public class UserXpLog
{
    [Key] 
    public Guid XpLogId { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }

    [MaxLength(255)]
    public string Source { get; set; } = string.Empty;

    public int Amount { get; set; }

    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}