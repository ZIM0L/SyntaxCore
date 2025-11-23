using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.UserRelated
{
    [Table("UserLoginHistories")]
    [Comment("Records of user login activities, including timestamps and device information.")]
    public class UserLoginHistory
    {
        [Key]
        public Guid LoginId { get; set; } = Guid.NewGuid();

        [ForeignKey(nameof(User))]
        public Guid UserId { get; set; }

        public DateTime LoginAt { get; set; }

        [MaxLength(255)]
        public string? IpAddress { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? DeviceInfo { get; set; } = string.Empty;

        public User User { get; set; } = null!;
    }
}
