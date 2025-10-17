using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SyntaxCore.Entities.UserRelated;

[Table("Achievements")]
public class Achievement
{
    [Key]
    public int AchievementId { get; set; }

    [MaxLength(255)]
    public string Name { get; set; }

    public int XpReward { get; set; }

    [MaxLength(255)]
    public string Icon { get; set; }

    public ICollection<UserAchievement> UserAchievements { get; set; }
}