using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.UserRelated;

[Table("UserLogins")]
public class UserLogin
{
    [Key]
    public int LoginId { get; set; }

    public int UserId { get; set; }

    public DateTime LoginAt { get; set; }

    [MaxLength(255)]
    public string IpAddress { get; set; }

    [MaxLength(255)]
    public string DeviceInfo { get; set; }

    // Navigation
    public User User { get; set; }
}