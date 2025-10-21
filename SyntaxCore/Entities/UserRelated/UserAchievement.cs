using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SyntaxCore.Entities.UserRelated
{
    [Table("UserAchievements")]
    [Comment("Tracks user progress towards achievements.")]
    [PrimaryKey(nameof(UserFK), nameof(AchievementFK))]
    public class UserAchievement
    {
        public Guid UserFK { get; set; }

        public Guid AchievementFK { get; set; }

        public int ProgressCurrent { get; set; }
        public int ProgressGoal { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime EarnedAt { get; set; }

        // Navigation
        [ForeignKey(nameof(UserFK))]
        public User User { get; set; } = null!;
        [ForeignKey(nameof(AchievementFK))]
        public Achievement Achievement { get; set; } = null!;
    }
}
