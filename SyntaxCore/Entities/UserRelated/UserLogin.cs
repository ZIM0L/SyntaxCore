using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.UserRelated;

[Table("UserLogins")]
[Comment("Records of user login activities, including timestamps and device information.")]
public class UserLogin
{
    [Key]
    public Guid LoginId { get; set; } = Guid.NewGuid();

    public Guid UserFK { get; set; }

    public DateTime LoginAt { get; set; }

    [MaxLength(255)]
    public string IpAddress { get; set; } = string.Empty;

    [MaxLength(255)]
    public string DeviceInfo { get; set; } = string.Empty;

    // Navigation
    [ForeignKey(nameof(UserFK))]
    public User User { get; set; } = null!;
}