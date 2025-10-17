using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.UserRelated
{
    [Table("UserLoginHistories")]
    public class UserLoginHistory
    {
        [Key]
        public int LoginId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public DateTime LoginAt { get; set; }

        [MaxLength(255)]
        public string IpAddress { get; set; }

        [MaxLength(255)]
        public string DeviceInfo { get; set; }

        public User User { get; set; }
    }
}
