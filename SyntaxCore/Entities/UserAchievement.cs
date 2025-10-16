using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities
{
    [Table("UserAchievements")]
    [Comment("Tracks user progress towards achievements.")]
    [PrimaryKey(nameof(UserId), nameof(AchievementId))]
    public class UserAchievement
    {
        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        public int AchievementId { get; set; }

        public int ProgressCurrent { get; set; }
        public int ProgressGoal { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? EarnedAt { get; set; }

        public User User { get; set; }
        //public Achievement Achievement { get; set; }
    }
}
